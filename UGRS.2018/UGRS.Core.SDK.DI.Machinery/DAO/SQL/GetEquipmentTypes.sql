SELECT T1.Code, T1.U_TypeEquip AS TypeEquipId, T2.Name AS TypeEquipName, T1.U_TypeContract AS TypeContractId FROM [@UG_MQ_TYCN] T1
INNER JOIN [@UG_GLO_EQUIPO] T2 
	ON T1.U_TypeEquip = T2.Code
WHERE T1.U_TypeContract = '{ContractCode}' GROUP BY T1.U_TypeEquip,T1.Code, T2.Name, T1.U_TypeContract