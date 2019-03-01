SELECT T2.AcctCode FROM [@UG_Config] T1 
INNER JOIN OACT T2
	ON T1.U_Value = T2.FormatCode 
WHERE T1.Name = '{ParameterName}' 