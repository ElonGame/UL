#pragma once

#include "Common.h"


enum ECode:uint8
{
	nop,	//ʲôҲ������ʹ���������+1�����������ֽ���
	ldc, //��һ����ֵ����ֵѹջ
	stloc,		//��ջ�ϵ�ֵ���ر��ر���
	starg,		//��ջ�ϵ�ֵ�洢������
	ret,		//����ָ����ݵ�ǰ���õĺ����Ƿ��з���ֵ����ָ���ǰջ�ϵķ���ֵ�����õ�������ջ��
	call,		//����ָ��
	ldarg,		//����һ��������ջ
	ldarga,		//���ز�����ַ�����������ô��ݲ�����
	ldloc,		//���ر��ر�����ջ
	ldloca,		//���ر��ر����ĵ�ַ��ջ��
	stind,		//�洢ֵ����ӵ�ַ
	initobj,	//��ʼ����ַ����ֵ
	newobj,		//�½�һ�����󲢵��ù��캯��
	box,		//��ջ�ϵ�ֵ����װ��Ϊָ�������ͣ�������������ͣ��򲻱�
	unbox,		//��ֵ���Ͷ����ֵ��ַ�洢��ջ
	unboxany,	//��ֵ���Ͳ���
	ldobj,		//����ַָ���ֵ���ص�ջ
};

namespace Command
{
	struct NOP
	{
		ECode Code;
		RET()
		{
			Code = ECode::nop;
		}
	};

	

	struct LDC
	{
		enum EValueType
		{
			Int32,
			Int64,
			Float32,
			Float64
		};
		ECode Code;
		EValueType Type;
		//Ҫ����ջ��ֵ
		union UValue
		{
			uint64 IValue;
			float64 FValue;
		}Value;
		
		LDC()
		{
			Code = ECode::ldc;
			Type = Int32;
			Value.IValue = 0;
		}
	};

	struct STLOC
	{
		ECode Code;
		uint16 Index;
		STLOC()
		{
			Code = ECode::stloc;
			Index = 0;
		}
	};
	struct STARG
	{
		ECode Code;
		uint16 Index;
		STLOC()
		{
			Code = ECode::starg;
			Index = 0;
		}
	};

	struct RET
	{
		ECode Code;
		RET()
		{
			Code = ECode::ret;
		}
	};

	struct CALL
	{
		ECode Code;
		int32 MethodHandle;
		CALL()
		{
			Code = ECode::call;
			Method = 0;
		}
	};

	struct LDARG
	{
		ECode Code;
		uint8 Index;
		LDARG()
		{
			Code = ECode::ldarg;
			Index = 0;
		}
	};

	struct LDARGA
	{
		ECode Code;
		uint8 Index;
		LDARG()
		{
			Code = ECode::ldarga;
			Index = 0;
		}
	};

	struct  LDLOC
	{
		ECode Code;
		uint8 Index;
		LDLOC()
		{
			Code = ECode::ldloc;
			Index = 0;
		}
	};

	struct LDLOCA
	{
		ECode Code;
		uint8 Index;
		LDLOCA()
		{
			Code = ECode::ldloca;
			Index = 0;
		}
	};

	struct STIND
	{
		enum EValueType
		{
			Int8,
			Int16,
			Int32,
			Int64,
			Float32,
			Float64,
			Ref,
		};

		ECode Code;
		EValueType Type;
		STIND()
		{
			Code = ECode::stind;
			Type = Int8;
		}
	};

	struct INITOBJ
	{
		ECode Code;
		int32 TypeHandle;
		INITOBJ()
		{
			Code = ECode::initobj;
			TypeHandle = 0;
		}
	};

	struct NEWOBJ
	{
		ECode Code;
		int32 TypeHandle;
		NEWOBJ()
		{
			Code = ECode::newobj;
			TypeHandle = 0;
		}
	};

	struct BOX
	{
		ECode Code;
		int32 TypeHandle;
		BOX()
		{
			Code = ECode::box;
			TypeHandle = 0;
		}
	};
}
//
//class Command
//{
//	public ECode Code;
//};
//
//class Command_Invoke :public Command
//{
//public:
//	//ע�ắ����ַ
//	int FuncID;
//	Command_Invoke() { Code = ECode.Invoke; }
//};
//
//class Command_Call :public Command
//{
//public:
//	//�����ַ
//	int CodeID;
//	Command_Call() { Code = ECode.Call; }
//};
//
//class Command_Ret :public Command
//{
//public:
//	Command_Ret() { Code = ECode.Ret; }
//};
//
//class Command_Push :public Command
//{
//public:
//	public Metadata.Const Value;
//	public Command_Push() { Code = ECode.Push; }
//};
//
//class Command_Pop :public Command
//{
//public:
//	//public int MemoryID; //�ڴ��ַ(Ϊ0��popһ�����󣬲���ŵ��ڴ�)
//	int Count;   //Pop����
//	Command_Pop() { Code = ECode.Pop; }
//};
//
//class Command_Move :public Command
//{
//	
//public:
//	enum AddressType
//	{
//		EBP,
//		ESP
//	};
//
//	AddressType SourceType;
//	int SourceIndex;  //�����EBP
//	AddressType DestType;
//	int DestIndex; //�����EBP
//	Command_Move() { Code = ECode.Move; }
//};
//
//class Command_Jump :public Command
//{
//	//�����ַ
//public:
//	int CodeID;
//	Command_Jump() { Code = ECode.Jump; }
//};
//
//class Command_JZ :public Command
//{
//	//�����ַ
//public:
//	int CodeID;
//	Command_JZ() { Code = ECode.JZ; }
//};

class Runtime
{

};