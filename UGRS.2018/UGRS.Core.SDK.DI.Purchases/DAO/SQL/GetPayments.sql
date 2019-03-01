
--select T0.U_GLO_CodeMov, DocEntry, DocNum, U_FZ_Auxiliar, CardName, U_GLO_CostCenter, DocDate, DocTotal, A1.GLFSV, A2.GLSSV, A3.U_Total, A3.U_Status
--from OVPM T0 
--left join (select sum(DocTotal) as GLFSV, U_GLO_CodeMov  from OVPM where U_GLO_PaymentType = 'GLFSV' and  DocType = 'A' and Canceled = 'N' group by U_GLO_CodeMov)A1
--on T0.U_GLO_CodeMov = A1.U_GLO_CodeMov
--left join(select sum(DocTotal) as GLSSV, U_GLO_CodeMov  from ORCT where U_GLO_PaymentType = 'GLSSV' and  DocType = 'A' and Canceled = 'N' group by U_GLO_CodeMov)A2
--on T0.U_GLO_CodeMov = A2.U_GLO_CodeMov
--left join(select U_CodeMov, sum(DE.U_Total)as U_Total, UC.U_Status from [@UG_GLO_VOUC] as UC left join [@UG_GLO_VODE] as DE on U_CodeVoucher = UC.Code and (DE.U_Status is null  or DE.U_Status != 'Cancelado')
--group by U_CodeMov, UC.U_Status)A3
--on T0.U_GLO_CodeMov = A3.U_CodeMov
--where DocType = 'A' and U_GLO_PaymentType = 'GLSOV' and Canceled = 'N' and T0.U_GLO_CodeMov is not null





select T0.U_GLO_CodeMov,/* DocEntry, DocNum,*/ U_FZ_Auxiliar, CardName, U_GLO_CostCenter, /*DocDate*/ DocTotal, isnull(A1.GLFSV,0) + isnull(B1.GLFSV,0) as GLFSV, A2.GLSSV, A3.U_Total, A3.U_Status, MQ1.Debit, MQ1.Credit
from OVPM T0 with (Nolock)
left join (

	select Sum(A1.Debit)-Sum(A1.Credit) as GLFSV,A1.U_GLO_CodeMov from OJDT A0 with (Nolock)
	inner join JDT1 A1 with (Nolock) on A0.TransId = A1.TransId  
	inner join [@UG_GLO_VODE] A2 with (Nolock) on A2.U_DocEntry = A1.TransId and A2.U_Type = 'Nota' 
	inner join [@UG_GLO_VOUC] A3 with (Nolock) on A2.U_CodeVoucher = A3.Code and A3.U_TypeVoucher=0
	inner join [@UG_Config] A4 with (Nolock) on A4.Name = 'MQ_DEUDORESVIAT' and A1.Account = A4.U_Value
	full outer join OJDT A5 with (Nolock) on A5.StornoToTr = A0.TransId 
	where A0.AutoStorno='N' and A5.TransId is null
	group by A1.U_GLO_CodeMov
)A1
on T0.U_GLO_CodeMov = A1.U_GLO_CodeMov
left join (select sum(DocTotal) as GLFSV, U_GLO_CodeMov  from OVPM with (Nolock) where U_GLO_PaymentType = 'GLREG' and  DocType = 'A' and Canceled = 'N' group by U_GLO_CodeMov)B1
on T0.U_GLO_CodeMov = B1.U_GLO_CodeMov
left join(select sum(DocTotal) as GLSSV, U_GLO_CodeMov  from ORCT with (Nolock) where U_GLO_PaymentType = 'GLSSV' and  DocType = 'A' and Canceled = 'N' group by U_GLO_CodeMov)A2
on T0.U_GLO_CodeMov = A2.U_GLO_CodeMov


left join (select sum(Debit) as Debit, sum(Credit) as Credit, U_GLO_CodeMov from JDT1 (Nolock) where TransCode = 'MQCM' group by U_GLO_CodeMov)MQ1
on T0.U_GLO_CodeMov = MQ1.U_GLO_CodeMov

left join(select U_CodeMov, UC.U_Total as U_Total, UC.U_Status from [@UG_GLO_VOUC]  as UC with (Nolock) left join [@UG_GLO_VODE] as DE on U_CodeVoucher = UC.Code and (DE.U_Status is null  or DE.U_Status = 'Cerrado')
group by U_CodeMov, UC.U_Total, UC.U_Status)A3
on T0.U_GLO_CodeMov = A3.U_CodeMov
where DocType = 'A' and U_GLO_PaymentType = 'GLSOV' and Canceled = 'N' and T0.U_GLO_CodeMov is not null

