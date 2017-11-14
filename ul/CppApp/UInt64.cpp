#include "stdafx.h"
#include "UInt64.h"
#include "Object.h"
#include "String.h"
#include "Boolean.h"
System::UInt64 System::UInt64::MaxValue=0xFFFFFFFFFFFFFFFF;
System::UInt64 System::UInt64::MinValue=0;
System::Boolean System::UInt64::TryParse(Ref<System::String>  value,System::UInt64  v)
{
	try
	{
		v.op_Assign(Parse(value));
		return true;
	}
	catch(System::Exception e)
	{
		v.op_Assign(0);
		return false;
	}
}
