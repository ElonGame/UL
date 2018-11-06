require "ul.System"
ul.System.Int16 = class('ul.System.Int16',ul.System.ValueType)
function ul.System.Int16.Parse_ul_System_String(value)
end
function ul.System.Int16:ToString()
end
function ul.System.Int16.TryParse_ul_System_String_ul_System_Int16(value,v,ref_func)
	do
		local __ret_v = try(
        function()
			do
				v = ul.System.Int16.Parse_ul_System_String(value);
				ref_func(v);
				return ul.System.Boolean.new(true);
			end
		end,
		{
			type="ul.System.Exception",
			func= function()
				do
					v = ul.System.Int32.Int16_ul_System_Int32(ul.System.Int32.new(0));
					ref_func(v);
					return ul.System.Boolean.new(false);
				end
			end
		}
		);
		if __ret_v~= nil then return __ret_v end 
	end
end
function ul.System.Int16.op_Addition(a,b)
end
function ul.System.Int16.op_Substraction(a,b)
end
function ul.System.Int16.op_Multiply(a,b)
end
function ul.System.Int16.op_Division(a,b)
end
function ul.System.Int16.op_Modulus(a,b)
end
function ul.System.Int16.op_BitwiseAnd(a,b)
end
function ul.System.Int16.op_BitwiseOr(a,b)
end
function ul.System.Int16.op_GreaterThen(a,b)
end
function ul.System.Int16.op_LessThen(a,b)
end
function ul.System.Int16.op_OnesComplement(a)
end
function ul.System.Int16.op_LeftShift(a,b)
end
function ul.System.Int16.op_RightShift(a,b)
end
function ul.System.Int16.op_Equality(a,b)
end
function ul.System.Int16.op_Inequality(a,b)
end
function ul.System.Int16.op_Increment(a)
end
function ul.System.Int16.op_Decrement(a)
end
function ul.System.Int16.Int64_ul_System_Int16(v)
end
function ul.System.Int16.Int32_ul_System_Int16(v)
end
function ul.System.Int16.Single_ul_System_Int16(v)
end
function ul.System.Int16.Double_ul_System_Int16(v)
end
function ul.System.Int16.Byte_ul_System_Int16(v)
end
function ul.System.Int16.SByte_ul_System_Int16(v)
end
function ul.System.Int16.UInt16_ul_System_Int16(v)
end
function ul.System.Int16.UInt32_ul_System_Int16(v)
end
function ul.System.Int16.UInt64_ul_System_Int16(v)
end
require "Int16_ExtHeader"
