using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace MTGLambda.MTGLambda.Helpers.Common
{
    public class ParamsWrapper<T> : IEnumerable<T>
    {
        private readonly IEnumerable<T> seq;

        public ParamsWrapper(IEnumerable<T> seq)
        {
            this.seq = seq;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="instance"></param>
        public static implicit operator ParamsWrapper<T>(T instance)
        {
            return new ParamsWrapper<T>(new[] { instance });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="seq"></param>
        public static implicit operator ParamsWrapper<T>(List<T> seq)
        {
            return new ParamsWrapper<T>(seq);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return seq.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return seq.GetEnumerator();
        }
    }
}
