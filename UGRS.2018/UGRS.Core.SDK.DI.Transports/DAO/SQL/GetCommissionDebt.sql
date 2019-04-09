	select 
		T1.Ref1, 
		Debit, 
		Credit, 
		U_GLO_Auxiliar 
	from JDT1 with (nolock) 
	inner join OJDT T1 with (nolock) on T1.TransID = JDT1.TransID
	full outer join OJDT T2 with (Nolock) on T2.StornoToTr = T1.TransId 
	where  
		T1.AutoStorno='N' 
		and T2.TransId is null 
		and T1.StornoToTr is null
		--and T1.Ref1 = '{Folio}' 
		and JDT1.U_GLO_CodeMov = '{Folio}' 
		and Account = '{Account}' 
		and U_GLO_TypeAux = '{TypeAux}' 
		and U_GLO_Auxiliar = '{Aux}'