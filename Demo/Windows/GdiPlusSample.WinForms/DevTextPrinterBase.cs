﻿//MIT, 2016-2017, WinterDev
using System.Collections.Generic;
using Typography.TextLayout;
namespace Typography.Rendering
{
    /// <summary>
    /// base TextPrinter class for developer only, 
    /// </summary>
    public abstract class DevTextPrinterBase
    {
        HintTechnique _hintTech;

        public DevTextPrinterBase()
        {
            FontSizeInPoints = 14;//
            ScriptLang = Typography.OpenFont.ScriptLangs.Latin;//default?
        }

        public abstract string FontFilename
        {
            get;
            set;
        }
        public abstract GlyphLayout GlyphLayoutMan { get; }
        public abstract Typography.OpenFont.Typeface Typeface { get; }
        public bool FillBackground { get; set; }
        public bool DrawOutline { get; set; }
        public float FontAscendingPx { get; set; }
        public float FontDescedingPx { get; set; }
        public float FontLineGapPx { get; set; }
        public float FontLineSpacingPx { get; set; }

        public HintTechnique HintTechnique
        {
            get { return _hintTech; }
            set
            {
                this._hintTech = value;
            }
        }


        float _fontSizeInPoints;
        public float FontSizeInPoints
        {
            get { return _fontSizeInPoints; }
            set
            {
                if (_fontSizeInPoints != value)
                {
                    _fontSizeInPoints = value;
                    OnFontSizeChanged();
                }
            }
        }

        protected virtual void OnFontSizeChanged() { }
        public Typography.OpenFont.ScriptLang ScriptLang { get; set; }
        public Typography.TextLayout.PositionTechnique PositionTechnique { get; set; }
        public bool EnableLigature { get; set; }
        public abstract void DrawString(char[] textBuffer, int startAt, int len, float xpos, float ypos);
        public abstract void DrawGlyphPlanList(List<GlyphPlan> glyphPlanList, float xpos, float ypos);

      
        public void DrawString(char[] textBuffer, float xpos, float ypos)
        {
            this.DrawString(textBuffer, 0, textBuffer.Length, xpos, ypos);
        }

        public abstract void DrawCaret(float xpos, float ypos);
        
    }

    public struct MeasureStringSize
    {
        public float Width;
        public float Height;
        public MeasureStringSize(float w, float h)
        {
            this.Width = w;
            this.Height = h;
        }
    }
}