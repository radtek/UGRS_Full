SELECT T1.Code, T1.Name, T1.U_Town, T2.Name AS TownName, T1.U_Kilometros FROM [@UG_GR_ROAD] T1 
INNER JOIN [@UG_GLO_TOWN] T2
	ON T1.U_Town = T2.Code
WHERE T1.U_Town = '{MunicipalityCode}'