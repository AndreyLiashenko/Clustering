﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.ML;
using Microsoft.ML.Data;

namespace Samples.Dynamic
{
    //public class SimpleDataViewImplementation
    //{
        public sealed class InputObject
        {
            public bool Label { get; }
            public string Text { get; }

            public InputObject(bool label, string text)
            {
                Label = label;
                Text = text;
            }
        }

        public sealed class InputObjectDataView : IDataView
        {
            private readonly IEnumerable<InputObject> _data;
            public DataViewSchema Schema { get; }
            public bool CanShuffle => false;

            public InputObjectDataView(IEnumerable<InputObject> data)
            {
                _data = data;

                var builder = new DataViewSchema.Builder();
                builder.AddColumn("Label", BooleanDataViewType.Instance);
                builder.AddColumn("Text", TextDataViewType.Instance);
                Schema = builder.ToSchema();
            }

            public long? GetRowCount() => null;

            public DataViewRowCursor GetRowCursor(
                IEnumerable<DataViewSchema.Column> columnsNeeded,
                Random rand = null)

                => new Cursor(this, columnsNeeded.Any(c => c.Index == 0),
                    columnsNeeded.Any(c => c.Index == 1));

            public DataViewRowCursor[] GetRowCursorSet(
                IEnumerable<DataViewSchema.Column> columnsNeeded, int n,
                Random rand = null)

                => new[] { GetRowCursor(columnsNeeded, rand) };

            private sealed class Cursor : DataViewRowCursor
            {
                private bool _disposed;
                private long _position;
                private readonly IEnumerator<InputObject> _enumerator;
                private readonly Delegate[] _getters;

                public override long Position => _position;
                public override long Batch => 0;
                public override DataViewSchema Schema { get; }

                public Cursor(InputObjectDataView parent, bool wantsLabel,
                    bool wantsText)

                {
                    Schema = parent.Schema;
                    _position = -1;
                    _enumerator = parent._data.GetEnumerator();
                    _getters = new Delegate[]
                    {
                        wantsLabel ?
                            (ValueGetter<bool>)LabelGetterImplementation : null,

                        wantsText ?
                            (ValueGetter<ReadOnlyMemory<char>>)
                            TextGetterImplementation : null

                    };
                }

                protected override void Dispose(bool disposing)
                {
                    if (_disposed)
                        return;
                    if (disposing)
                    {
                        _enumerator.Dispose();
                        _position = -1;
                    }
                    _disposed = true;
                    base.Dispose(disposing);
                }

                private void LabelGetterImplementation(ref bool value)
                    => value = _enumerator.Current.Label;

                private void TextGetterImplementation(
                    ref ReadOnlyMemory<char> value)

                    => value = _enumerator.Current.Text.AsMemory();

                private void IdGetterImplementation(ref DataViewRowId id)
                    => id = new DataViewRowId((ulong)_position, 0);

                public override ValueGetter<TValue> GetGetter<TValue>(
                    DataViewSchema.Column column)

                {
                    if (!IsColumnActive(column))
                        throw new ArgumentOutOfRangeException(nameof(column));
                    return (ValueGetter<TValue>)_getters[column.Index];
                }

                public override ValueGetter<DataViewRowId> GetIdGetter()
                    => IdGetterImplementation;

                public override bool IsColumnActive(DataViewSchema.Column column)
                    => _getters[column.Index] != null;

                public override bool MoveNext()
                {
                    if (_disposed)
                        return false;
                    if (_enumerator.MoveNext())
                    {
                        _position++;
                        return true;
                    }
                    Dispose();
                    return false;
                }
            }

        }
    }
//}