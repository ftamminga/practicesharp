﻿#region © Copyright 2010 Yuval Naveh, Practice Sharp. LGPL.
/* Practice Sharp
 
    © Copyright 2010, Yuval Naveh.
     All rights reserved.
 
    This file is part of Practice Sharp.

    Practice Sharp is free software: you can redistribute it and/or modify
    it under the terms of the GNU Lesser Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    Practice Sharp is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU Lesser Public License for more details.

    You should have received a copy of the GNU Lesser Public License
    along with Practice Sharp.  If not, see <http://www.gnu.org/licenses/>.
*/
#endregion

#region Original License
// Copyright 2006, Thomas Scott Stillwell
// All rights reserved.
//
//Redistribution and use in source and binary forms, with or without modification, are permitted 
//provided that the following conditions are met:
//
//Redistributions of source code must retain the above copyright notice, this list of conditions 
//and the following disclaimer. 
//
//Redistributions in binary form must reproduce the above copyright notice, this list of conditions 
//and the following disclaimer in the documentation and/or other materials provided with the distribution. 
//
//The name of Thomas Scott Stillwell may not be used to endorse or 
//promote products derived from this software without specific prior written permission. 
//
//THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR 
//IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND 
//FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS 
//BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES 
//(INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR 
//PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, 
//STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF 
//THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

// ++ ported to .NET by Mark Heath ++

#endregion

using System;
using System.Collections.Generic;
using System.Text;

namespace BigMansStuff.PracticeSharp.Core
{
    /// <summary>
    /// Base class for all DSP effects
    /// Based on the Slider class written by Mark Heath, SkypeFX
    /// </summary>
    public abstract class DSPEffect
    {
        protected List<DSPEffectFactor> m_factors;
        public float SampleRate { get; set; }
        public float Tempo { get; set; }
        public bool Enabled { get; set; }

        public DSPEffect()
        {
            m_factors = new List<DSPEffectFactor>();
            Enabled = true;
            Tempo = 120;
            SampleRate = 44100;
        }

        public IList<DSPEffectFactor> Factors { get { return m_factors; } }

        public DSPEffectFactor AddFactor(float defaultValue, float minimum, float maximum, float increment, string description)
        {
            DSPEffectFactor factor = new DSPEffectFactor(defaultValue, minimum, maximum, increment, description);
            m_factors.Add(factor);
            return factor;
        }

        public abstract string Name { get; }

        // helper base methods
        // these are primarily to enable derived classes to use a similar
        // syntax to JS effects
        protected float Factor1 { get { return m_factors[0].Value; } }
        protected float Factor2 { get { return m_factors[1].Value; } }
        protected float Factor3 { get { return m_factors[2].Value; } }
        protected float Factor4 { get { return m_factors[3].Value; } }
        protected float Factor5 { get { return m_factors[4].Value; } }
        protected float Factor6 { get { return m_factors[5].Value; } }
        protected float Factor7 { get { return m_factors[6].Value; } }
        protected float Factor8 { get { return m_factors[7].Value; } }
        protected float Min(float a, float b) { return Math.Min(a, b); }
        protected float Max(float a, float b) { return Math.Max(a, b); }
        protected float Abs(float a) { return Math.Abs(a); }
        protected float Exp(float a) { return (float)Math.Exp(a); }
        protected float Sqrt(float a) { return (float)Math.Sqrt(a); }
        protected float Sin(float a) { return (float)Math.Sin(a); }
        protected float Tan(float a) { return (float)Math.Tan(a); }
        protected float Cos(float a) { return (float)Math.Cos(a); }
        protected float Pow(float a, float b) { return (float)Math.Pow(a, b); }
        protected float Sign(float a) { return Math.Sign(a); }
        protected float Log(float a) { return (float)Math.Log(a); }
        //protected float PI { get { return (float)Math.PI; } }

        protected const float Db2log = 0.11512925464970228420089957273422f; // ln(10) / 20 
        protected const float PI = 3.1415926535f;
        protected const float HalfPi = 1.57079632675f; // pi / 2;
        protected const float HalfPiScaled = 2.218812643387445f; // halfpi * 1.41254f;


        protected void Convolve_c(float[] buffer1, int offset1, float[] buffer2, int offset2, int count)
        {
            for (int i = 0; i < count * 2; i += 2)
            {
                float r = buffer1[offset1 + i];
                float im = buffer1[offset1 + i + 1];
                float cr = buffer2[offset2 + i];
                float ci = buffer2[offset2 + i + 1];
                buffer1[offset1 + i] = r * cr - im * ci;
                buffer1[offset1 + i + 1] = r * ci + im * cr;
            }
        }

        /// <summary>
        /// Should be called on effect load, sample rate changes, and start of playback
        /// </summary>
        public virtual void Init()
        {
        }

        /// <summary>
        /// will be called when a factor value has been changed
        /// </summary>
        public abstract void OnFactorChanges();

        /// <summary>
        /// called before each block is processed
        /// </summary>
        /// <param name="samplesblock">number of samples in this block</param>
        public virtual void Block(int samplesblock)
        { 
        }

        /// <summary>
        /// Processed a single sample - should be called for each sample
        /// </summary>        
        public abstract void Sample(ref float spl0, ref float spl1);

        public override string ToString()
        {
            return Name;
        }
    }
}