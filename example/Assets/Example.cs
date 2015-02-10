﻿using UnityEngine;
using System;
using System.Collections;
using System.Runtime.InteropServices;
using NLua;
using System.Text;
using System.IO;
using System.Collections.Generic;


namespace X
{
    public class Example : MonoBehaviour
    {
        static string LuaString = @"
			function OnGUI()
				for i = 1, 7 do
					GUILayout.Label('Test' .. i,{ }) 
				end

				print('hello', 'world')
			end";
        public string Parameter;
		private LuaTable Libs;

        void Start()
        {
			mLuaState = new Lua();
			mLuaState.LoadCLRPackage();

			UnityLua.UnityExpand.Open(mLuaState.LuaState);

			mLuaState.DoString(LuaString);
        }

		void OnGUI() {
			Call("OnGUI");
		}
		
		public System.Object[] Call(string function, params System.Object[] args) 
		{
			System.Object[] result = new System.Object[0];
			if(mLuaState == null) 
				return result;

			LuaFunction lf = mLuaState.GetFunction(function);
			if(lf == null) 
				return result;

			try 
			{
				// Note: calling a function that does not 
				// exist does not throw an exception.
				if(args != null) 
				{
					result = lf.Call(args);
				} 
				else 
				{
					result = lf.Call();
				}
			} 
			catch(NLua.Exceptions.LuaException e) 
			{
				Debug.LogException(e, gameObject);
			}
			return result;
		}

        Lua mLuaState;
    }
}
