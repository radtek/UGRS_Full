SELECT 
	T1.U_Area + '_' + T1.U_Folio
FROM  
	[@UG_GLO_VODE] T0 with (Nolock)
	INNER JOIN [@UG_GLO_VOUC] T1  with (Nolock) ON T1.Code = T0.U_CodeVoucher 
WHERE U_DocEntry = '{DocEntry}'