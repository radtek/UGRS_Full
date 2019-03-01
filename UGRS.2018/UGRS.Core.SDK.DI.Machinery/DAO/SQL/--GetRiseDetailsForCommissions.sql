declare @RiseId as nvarchar(10);
declare @Account as nvarchar(30);
set @RiseId= '{RiseId}'
set @Account= '{Account}'

select CONCAT(A1.lastName, ' ', A1.firstName) AS EmployeeName, a0.horas * a2.U_Value AS Comision, a0.*, A2.U_Value AS Rate,
CASE WHEN a0.Tipo = 'S' THEN ISNULL(A3.ImportFS, 0) ELSE 0 END ImportFS,
ISNULL(A4.ImportFS, 0) Adeudo, ISNULL(A4.U_GLO_CodeMov, '') AS CodeMove
from
(select 'S' Tipo, s1.U_Supervisor AS EmpId, sum(s1.u_hrfeet) + isnull(s2.U_Hrs, 0) Horas from [@UG_MQ_RIHR] s1 with (nolock) left join [@UG_MQ_RIHT] s2 with (nolock) on s2.U_IdRise = @RiseId where s1.u_idrise= @RiseId group by s1.U_Supervisor, s2.U_Hrs union all
select 'O' Tipo, s1.U_Operator IDEmp, sum(s1.u_hrfeet) + isnull(s2.U_Hrs, 0) Horas from [@UG_MQ_RIHR] s1 with (nolock) left join [@UG_MQ_RIHT] s2 with (nolock) on s2.U_Operator = s1.U_Operator and s2.U_IdRise = @RiseId where s1.u_idrise= @RiseId group by s1.U_Operator, s2.U_Hrs) A0
/*(select 'S' Tipo, s1.U_Supervisor AS EmpId, sum(s1.u_hrfeet) + sum(isnull(s2.U_Hrs, 0)) Horas from [@UG_MQ_RIHR] s1 with (nolock) left join [@UG_MQ_RIHT] s2 with (nolock) on s2.U_Operator = s1.U_Supervisor and s2.U_IdRise = @RiseId where s1.u_idrise= @RiseId group by s1.U_Supervisor union all
select 'O' Tipo, s1.U_Operator IDEmp, sum(s1.u_hrfeet) + sum(isnull(s2.U_Hrs, 0)) Horas from [@UG_MQ_RIHR] s1 with (nolock) left join [@UG_MQ_RIHT] s2 with (nolock) on s2.U_Operator = s1.U_Operator and s2.U_IdRise = @RiseId where s1.u_idrise= @RiseId group by s1.U_Operator) A0*/
/*(select 'S' Tipo,U_Supervisor AS EmpId, sum(u_hrfeet) Horas from [@UG_MQ_RIHR] with (nolock) where u_idrise= @RiseId group by U_Supervisor union all
select 'O' Tipo, U_Operator IDEmp, sum(u_hrfeet) Horas from [@UG_MQ_RIHR] with (nolock) where u_idrise= @RiseId group by U_Operator) A0*/
inner join OHEM A1 on a0.EmpId = a1.empID
inner join 
(
   SELECT U_Value, case when name='MQ_SUP_COMMISSIONRATE' then 'S' else 'O' end Tipo from [@UG_CONFIG] with (nolock) where
   name in ('MQ_SUP_COMMISSIONRATE','MQ_OPR_COMMISSIONRATE')
) A2 on a2.Tipo = a0.Tipo
left join
(
    SELECT SUM(ISNULL(Debit, 0)) - SUM(ISNULL(Credit, 0)) ImportFS, U_GLO_Auxiliar, Account 
       FROM JDT1 with (nolock)
       WHERE U_GLO_CodeMov IN ( SELECT U_GLO_CodeMov 
                                   FROM OVPM with (nolock) 
                                WHERE Canceled = 'N' AND U_GLO_FolioOri = @RiseId
																	and 
                                                                     U_GLO_PaymentType='GLSOV' group by U_GLO_CodeMov) 
                                                       group by U_GLO_Auxiliar, Account
) A3 ON A3.U_GLO_Auxiliar = A0.EmpId and a3.Account = @Account
left join
(    Select sum(F0.ImportFS) ImportFS, F0.Account,f0.U_GLO_CodeMov,U_GLO_Auxiliar
     from (
          SELECT SUM(ISNULL(Debit, 0)) - SUM(ISNULL(Credit, 0)) ImportFS, Account,U_GLO_CodeMov, max(U_GLO_Auxiliar) U_GLO_Auxiliar
          FROM JDT1 with (nolock) inner join OJDT t2 with (nolock) on JDT1.TransId = t2.TransId
          WHERE U_GLO_CodeMov IN (SELECT top 1 U_GLO_CodeMov 
                                        FROM OVPM with (nolock)
                                  WHERE Canceled = 'N' AND U_GLO_FolioOri != @RiseId and docdate <= (select min(docdate) from ovpm where U_GLO_FolioOri = @RiseId )
                                        and U_GLO_PaymentType='GLSOV' and U_FZ_Auxiliar=(select u_supervisor from [@UG_TBL_MQ_RISE] A5 where a5.U_IdRise = @RiseId
										)order by docentry desc	
                ) group by U_GLO_Auxiliar, Account , U_GLO_CodeMov, t2.TaxDate, U_GLO_CodeMov,U_GLO_CodeMov
              having SUM(ISNULL(Debit, 0)) - SUM(ISNULL(Credit, 0)) >0
         ) F0 
         group by F0.Account,f0.U_GLO_CodeMov,U_GLO_Auxiliar
) A4 ON  a4.Account = @Account and a4.U_GLO_Auxiliar= a0.EmpId
