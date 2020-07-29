﻿//MIT, 2020-present, WinterDev
using System; 
using PixelFarm.CpuBlit;
using PixelFarm.CpuBlit.VertexProcessing; 

using PaintLab.Svg;

using LayoutFarm.WebLexer;
 

namespace PaintLab
{


    static class SvgBuilderHelper
    {
        static VgVisualDocHost _vgDocHost = new VgVisualDocHost();
        public static void ParseAndRenderSvg(PixelFarm.Drawing.SvgBmpBuilderReq req)
        {
            //----------
            //copy from HtmlRenderer's SvgViewer demo
            //----------  
            var docBuilder = new VgDocBuilder();
            var parser = new SvgParser(docBuilder);
            TextSnapshot textSnapshot = new TextSnapshot(req.SvgContent.ToString());
            parser.ParseDocument(textSnapshot);

            VgVisualDocBuilder builder = new VgVisualDocBuilder();
            VgVisualElement vgVisElem = builder.CreateVgVisualDoc(docBuilder.ResultDocument, _vgDocHost).VgRootElem;
            PixelFarm.CpuBlit.VertexProcessing.Q1RectD bounds = vgVisElem.GetRectBounds();

            float actualXOffset = (float)-bounds.Left;
            float actualYOffset = (float)-bounds.Bottom;

            //original svg width, height
            int bmpW = (int)Math.Round(bounds.Width);
            int bmpH = (int)Math.Round(bounds.Height);

            if (bmpW == 0 || bmpH == 0)
            {
                return;
            }

            //scale svg to specific size
            float scale_w = req.ExpectedWidth / bmpW;

            //at this point, we have 2 options
            //1) create bitmap with original svg size and scale it down to expected size
            //2) scale svg to expected size and create a bitmap

            //we choose 2) 

            int new_w = (int)Math.Round(bmpW * scale_w);
            int new_h = (int)Math.Round(bmpH * scale_w);

            MemBitmap memBitmap = new MemBitmap(new_w, new_h);
            using (Tools.BorrowAggPainter(memBitmap, out var p))
            using (Tools.More.BorrowVgPaintArgs(p, out VgPaintArgs paintArgs))
            {
                //pass by 

                Affine tx = Affine.NewScaling(scale_w);
                paintArgs._currentTx = tx;

                float orgX = p.OriginX;
                float orgY = p.OriginY;


                p.SetOrigin(actualXOffset * scale_w, actualYOffset * scale_w);

                p.Clear(req.DefaultBgColor);

                p.FillColor = PixelFarm.Drawing.Color.Black;

                double prevStrokeW = p.StrokeWidth;

                vgVisElem.Paint(paintArgs);

                p.StrokeWidth = prevStrokeW;//restore 

                p.SetOrigin(orgX, orgY);//restore
            }

#if DEBUG
            //memBitmap.SaveImage("svg.png");
#endif
            req.Output = memBitmap;
        }


    }
}