using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

//public delegate void DelegateString(string str);
//public delegate void DelegatePointerEventData(PointerEventData pointerEventData);
//public delegate void DelegateBool(bool b);
//public delegate void DelegateBoolBool(bool bVal1, bool bVal2);
//public delegate void DelegateInt(int n);

public delegate void Delegate();
public delegate R DelegateR<R>();

public delegate void Delegate<T1>(T1 t1);
public delegate void Delegate<T1, T2>(T1 t1, T2 t2);
public delegate void Delegate<T1, T2, T3>(T1 t1, T2 t2, T3 t3);
public delegate void Delegate<T1, T2, T3, T4>(T1 t1, T2 t2, T3 t3, T4 t4);
public delegate void Delegate<T1, T2, T3, T4, T5>(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5);
public delegate void Delegate<T1, T2, T3, T4, T5, T6>(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6);
public delegate void Delegate<T1, T2, T3, T4, T5, T6, T7>(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7);
public delegate void Delegate<T1, T2, T3, T4, T5, T6, T7, T8>(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8);
public delegate void Delegate<T1, T2, T3, T4, T5, T6, T7, T8, T9>(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9);
public delegate void Delegate<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10);
public delegate void Delegate<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11);

public delegate R DelegateR<R, T1>(T1 t1);
public delegate R DelegateR<R, T1, T2>(T1 t1, T2 t2);
public delegate R DelegateR<R, T1, T2, T3>(T1 t1, T2 t2, T3 t3);
public delegate R DelegateR<R, T1, T2, T3, T4>(T1 t1, T2 t2, T3 t3, T4 t4);
public delegate R DelegateR<R, T1, T2, T3, T4, T5>(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5);
public delegate R DelegateR<R, T1, T2, T3, T4, T5, T6>(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6);
public delegate R DelegateR<R, T1, T2, T3, T4, T5, T6, T7>(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7);
public delegate R DelegateR<R, T1, T2, T3, T4, T5, T6, T7, T8>(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8);
public delegate R DelegateR<R, T1, T2, T3, T4, T5, T6, T7, T8, T9>(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9);
public delegate R DelegateR<R, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10);


