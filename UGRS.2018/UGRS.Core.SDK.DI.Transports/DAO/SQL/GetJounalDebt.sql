
	select Ref1, Debit, Credit from JDT1 with (nolock) where Ref1 = '{Folio}' and Account = '{Account}' and U_GLO_TypeAux = '{TypeAux}' and U_GLO_Auxiliar = '{Aux}'