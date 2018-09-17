﻿//MIT, 2016-present, WinterDev
// some code from icu-project
// © 2016 and later: Unicode, Inc. and others.
// License & terms of use: http://www.unicode.org/copyright.html#License

using System;
using System.Collections.Generic;

namespace Typography.TextBreak
{
    public enum VisitorState
    {
        Init,
        Parsing,
        OutOfRangeChar,
        End,
    }



    public ref struct WordVisitor
    {
        readonly ReadOnlySpan<char> _buffer;

        public WordVisitor(ReadOnlySpan<char> buffer, ICollection<BreakAtInfo> breakAtList, Stack<int> tempCandidateBreaks)
        {
            _buffer = buffer;
            BreakList = breakAtList;
            TempCandidateBreaks = tempCandidateBreaks;
            CurrentIndex = 0;
            State = VisitorState.Init;
            LatestBreakAt = 0;
        }

        public VisitorState State { get; set; }
        public int CurrentIndex { get; private set; }
        public int LatestBreakAt { get; private set; }
        public ICollection<BreakAtInfo> BreakList { get; }
        internal Stack<int> TempCandidateBreaks { get; }

        public char CurrentChar => _buffer[CurrentIndex];
        public bool IsEnd => CurrentIndex >= _buffer.Length;
        
        public void AddWordBreakAt(int index, WordKind wordKind)
        {
            if (index == LatestBreakAt)
                throw new InvalidOperationException("The last break index was the same as the current one.");

            BreakList.Add(new BreakAtInfo(index, wordKind));
            this.LatestBreakAt = index;
        }
        public void AddWordBreakAtCurrentIndex(WordKind wordKind = WordKind.Text) =>
            AddWordBreakAt(this.CurrentIndex, wordKind);
        public void SetCurrentIndex(int index)
        {
            this.CurrentIndex = index;
            if (IsEnd) //if can't read next then set state to end
                this.State = VisitorState.End;
        }
        public void Break(BreakBounds bb)
        {
            AddWordBreakAt(bb.startIndex + bb.length, bb.kind);
            SetCurrentIndex(LatestBreakAt);
        }
    }
}