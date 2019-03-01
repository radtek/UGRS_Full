select DISTINCT * from (select T1.OcrCode2 from OPCH T0 with (nolock) inner join PCH1 T1 with (nolock) on t0.DocEntry= t1.DocEntry where T1.OcrCode2 is not null and T0.U_MQ_Rise = '{RiseId}' and t1.Quantity>0 and t0.CANCELED='N' union all
select T1.OcrCode2 from OWTR T0 with (nolock) inner join WTR1 T1 with (nolock) on t0.DocEntry= t1.DocEntry where T1.OcrCode2 is not null and T0.U_MQ_Rise = '{RiseId}' and t1.Quantity>0 and t0.CANCELED='N' union all
select T1.OcrCode2 from OWTQ T0 with (nolock) inner join WTQ1 T1 with (nolock) on t0.DocEntry= t1.DocEntry where T1.OcrCode2 is not null and T0.U_MQ_Rise = '{RiseId}' and t1.Quantity>0 union all
---- Anexa los AF relacionados
select T2.U_PrcCode from [@UG_TBL_MQ_RISE] T0 with (nolock) inner join [@UG_TBL_MQ_RISE] T1 with (nolock) on t0.U_DocRef = t1.U_IdRise inner join [@UG_MQ_RIFR] T2 with (nolock) on t1.U_IdRise= t2.U_IdRise where T0.U_IdRise = '{RiseId}') a
