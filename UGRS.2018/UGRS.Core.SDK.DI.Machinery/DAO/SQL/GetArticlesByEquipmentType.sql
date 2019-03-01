SELECT T1.Code, T1.U_ItemCode AS ItemCode, T2.ItemName, T1.U_TypeEquip AS TypeEquipId, T1.U_TypeContract AS TypeContractId, T3.U_UsePerfora FROM [@UG_MQ_TYCN] T1
INNER JOIN [OITM] T2
	ON T1.U_ItemCode = T2.ItemCode
INNER JOIN [@UG_GLO_EQUIPO] T3
	ON T1.U_TypeEquip = T3.Code
WHERE T1.U_TypeEquip = '{EquipmentTypeCode}' AND T1.U_TypeContract = '{ContractTypeCode}'