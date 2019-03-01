SELECT DISTINCT T2.Code, T2.Name, T2.U_Town, T2.Name AS TownName, T2.U_Kilometros 
FROM RDR1 T1
INNER JOIN [@UG_GR_ROAD] T2
	ON T1.U_GLO_Sections = T2.Code
INNER JOIN [@UG_GLO_TOWN] T3
	ON T2.U_Town = T3.Code
WHERE T1.DocEntry = '{DocEntry}'