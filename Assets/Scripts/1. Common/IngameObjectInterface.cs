using System;
using UnityEngine;

public interface INpcObjectInfo
{
	long GetInstanceId();
	CsNpcInfo GetNpcInfo();
	Transform GetTransform();
}

public interface IHeroObjectInfo
{
	Guid GetHeroId();
	CsHeroBase GetHeroBase();
	Transform GetTransform();
}

public interface IMonsterObjectInfo
{
	long GetInstanceId();
	CsMonsterInfo GetMonsterInfo();
	Transform GetTransform();
	int GetMaxHp();
	int GetHp();
	int GetMaxMentalStrength();
	int GetMentalStrength();
}

public interface ICartObjectInfo
{
	long GetInstanceId();
	CsCartObject GetCartObject();
	Transform GetTransform();
}

public interface IQuestObject
{
	int GetSpecificId();
}
