/********************************************************************************
 [ 文件 ] System单例类
 [ 作者 ] 肖文
 [ 日期 ] 2019/03/15
 [ 描述 ] 规范System管理类
 ********************************************************************************/
using System;
using System.Collections;
using System.Collections.Generic;

namespace WeFrame
{
    /// <summary>
    /// [ System单例类 ] 规范System管理类
    /// </summary>
    public abstract class SystemSingleton<T> : SystemObject where T : SystemSingleton<T>, new()
    {
        /// <summary>
        /// [ 单例 ]
        /// </summary>
        private static T mSelf = null;

        /// <summary>
        /// [ 单例 ]
        /// </summary>
        public static T Self
        {
            get
            {
                if (mSelf == null)
                {
                    mSelf = new T();
                }

                return mSelf;
            }
        }

        protected override void Release()
        {
            base.Release();
            
            mSelf = null;
        }
    }
}
