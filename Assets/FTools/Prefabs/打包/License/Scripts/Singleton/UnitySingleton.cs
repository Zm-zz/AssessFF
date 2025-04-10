/********************************************************************************
 [ 文件 ] Unity单例类
 [ 作者 ] 肖文
 [ 日期 ] 2018/03/15
 [ 描述 ] 规范Unity管理类
 ********************************************************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeFrame
{
    /// <summary>
    /// [ Unity单例类 ] 规范Unity管理类
    /// </summary>
    public abstract class UnitySingleton<T> : UnityObject where T : UnitySingleton<T>
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
            set
            {
                if (mSelf == null)
                {
                    mSelf = value;
                }
            }

            get
            {
                return mSelf;
            }
        }

        protected override void BeforeInit()
        {
            base.BeforeInit();

            mSelf = this as T;
        }

        protected override void Release()
        {
            base.Release();
            
            mSelf = null;
        }
    }    
}
