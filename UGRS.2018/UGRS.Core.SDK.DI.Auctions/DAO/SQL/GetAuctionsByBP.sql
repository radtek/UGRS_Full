

-- Subastas por Socio de negocio
	select U_SU_Folio, (sum(Debit)- sum(Credit)) as Compra from JDT1 A0
	inner join [@UG_CONFIG] on Name = Case when '{CostingCode}' = 'SU_OBREG' THEN 'SU_COMPRADORSS' ELSE 'SU_COMPRADOR' END  and U_Value = A0.Account
	where U_GLO_Auxiliar = '{CardCode}' 
	group by U_SU_Folio
	having (sum(Debit)- sum(Credit)) > 0 


