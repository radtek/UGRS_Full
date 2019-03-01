DECLARE @FolioOri NVARCHAR(10) 

SET @FolioOri= '{RiseId}' 

SELECT w0.ocrcode2      Equipo,
	   ISNULL(W7.Code, '') Code,
       Max(w5.frgnname) NumEconomico, 
       CASE 
         WHEN w0.tipo = 'A' THEN 'Traslado' 
         ELSE 'Compra' 
       END              Tipo, 
       w6.u_typeeq      EqType, 
       Sum(CASE 
             WHEN w4.qrygroup21 = 'Y' THEN 
               CASE 
                 WHEN w2.docentry IS NOT NULL THEN w2.quantity 
                 ELSE w3.quantity 
               END 
             ELSE 0 
           END)         Gasolina, 
       Sum(CASE 
             WHEN w4.qrygroup26 = 'Y' THEN 
               CASE 
                 WHEN w2.docentry IS NOT NULL THEN w2.quantity 
                 ELSE w3.quantity 
               END 
             ELSE 0 
           END)         '15W40', 
       Sum(CASE 
             WHEN w4.qrygroup28 = 'Y' THEN 
               CASE 
                 WHEN w2.docentry IS NOT NULL THEN w2.quantity 
                 ELSE w3.quantity 
               END 
             ELSE 0 
           END)         Hidra, 
       Sum(CASE 
             WHEN w4.qrygroup29 = 'Y' THEN 
               CASE 
                 WHEN w2.docentry IS NOT NULL THEN w2.quantity 
                 ELSE w3.quantity 
               END 
             ELSE 0 
           END)         SAE, 
       Sum(CASE 
             WHEN w4.qrygroup30 = 'Y' THEN 
               CASE 
                 WHEN w2.docentry IS NOT NULL THEN w2.quantity 
                 ELSE w3.quantity 
               END 
             ELSE 0 
           END)         TRANS, 
       Sum(CASE 
             WHEN w4.qrygroup23 = 'Y' THEN 
               CASE 
                 WHEN w2.docentry IS NOT NULL THEN w2.quantity 
                 ELSE w3.quantity 
               END 
             ELSE 0 
           END)         Grasa, 
       Sum(CASE 
             WHEN w4.qrygroup25 = 'Y' THEN 
               CASE 
                 WHEN w2.docentry IS NOT NULL THEN w2.quantity 
                 ELSE w3.quantity 
               END 
             ELSE 0 
           END)         DieselT 
FROM   (SELECT t0.docentry, 
               t1.ocrcode2, 
               'A' Tipo 
        FROM   owtr T0 
               INNER JOIN wtr1 t1 
                       ON t1.docentry = t0.docentry 
        WHERE  t1.whscode = 'MQHEOBRA' 
               AND T0.u_mq_rise = @FolioOri 
               AND t1.ocrcode2 IS NOT NULL 
               AND t1.quantity > 0 
               AND t0.canceled = 'N' 
        GROUP  BY t0.docentry, 
                  t1.ocrcode2 
        UNION ALL 
        SELECT t0.docentry, 
               t1.ocrcode2, 
               'C' Tipo 
        FROM   opch T0 
               INNER JOIN pch1 t1 
                       ON t0.docentry = t1.docentry 
        WHERE  t1.whscode = 'MQHEOBRA' 
               AND t0.canceled = 'N' 
               AND t0.u_mq_rise = @FolioOri 
               AND t1.ocrcode2 IS NOT NULL 
        GROUP  BY t0.docentry, 
                  t1.ocrcode2) W0 
       LEFT JOIN (SELECT T2.u_prccode 
                  FROM   [@ug_tbl_mq_rise] T0 WITH (nolock) 
                         INNER JOIN [@ug_tbl_mq_rise] T1 WITH (nolock) 
                                 ON t0.u_docref = t1.u_idrise 
                         INNER JOIN [@ug_mq_rifr] T2 WITH (nolock) 
                                 ON t1.u_idrise = t2.u_idrise 
                  WHERE  T0.u_idrise = @FolioOri) WW 
              ON Ww.u_prccode = w0.ocrcode2 
       LEFT JOIN (SELECT B0.docentry, 
                         B0.ocrcode2, 
                         B0.tipo 
                  FROM   (SELECT a1.docdate, 
                                 A1.docentry, 
                                 a0.tipo, 
                                 A0.ocrcode2 
                          FROM   (SELECT Min(t0.docentry) Folio, 
                                         t1.ocrcode2, 
                                         'A'              Tipo 
                                  FROM   owtr T0 
                                         INNER JOIN wtr1 t1 
                                                 ON t1.docentry = t0.docentry 
                                  WHERE  t1.whscode = 'MQHEOBRA' 
                                         AND T0.u_mq_rise = @FolioOri 
                                         AND t1.ocrcode2 IS NOT NULL 
                                         AND t1.quantity > 0 
                                         AND t0.canceled = 'N' 
                                  GROUP  BY t1.ocrcode2) A0 
                                 INNER JOIN owtr A1 
                                         ON a1.docentry = A0.folio 
                          UNION ALL 
                          SELECT a1.docdate, 
                                 A1.docentry, 
                                 a0.tipo, 
                                 A0.ocrcode2 
                          FROM   (SELECT Min(t0.docentry) Folio, 
                                         'C'              Tipo, 
                                         t1.ocrcode2 
                                  FROM   opch T0 
                                         INNER JOIN pch1 t1 
                                                 ON t0.docentry = t1.docentry 
                                  WHERE  t1.whscode = 'MQHEOBRA' 
                                         AND t0.canceled = 'N' 
                                         AND t0.u_mq_rise = @FolioOri 
                                         AND t1.ocrcode2 IS NOT NULL 
                                  GROUP  BY t1.ocrcode2) A0 
                                 INNER JOIN opch A1 
                                         ON a0.folio = a1.docentry) B0 
                         LEFT JOIN (SELECT a1.docdate, 
                                           A1.docentry, 
                                           a0.tipo, 
                                           A0.ocrcode2 
                                    FROM   (SELECT max(t0.docentry) Folio, 
                                                   'C'              Tipo, 
                                                   t1.ocrcode2 
                                            FROM   opch T0 
                                                   INNER JOIN pch1 t1 
                                                           ON t0.docentry = 
                                                              t1.docentry 
                                            WHERE  t1.whscode = 'MQHEOBRA' 
                                                   AND t0.canceled = 'N' 
                                                   AND t0.u_mq_rise = @FolioOri 
                                                   AND t1.ocrcode2 IS NOT NULL 
                                            GROUP  BY t1.ocrcode2) A0 
                                           INNER JOIN opch A1 
                                                   ON a0.folio = a1.docentry) B1 
                                ON b1.ocrcode2 = b0.ocrcode2 
                                   AND b0.tipo = 'A' 
                                   AND b1.docdate < b0.docdate 
                         LEFT JOIN (SELECT a1.docdate, 
                                           A1.docentry, 
                                           a0.tipo, 
                                           A0.ocrcode2 
                                    FROM   (SELECT max(t0.docentry) Folio, 
                                                   t1.ocrcode2, 
                                                   'A'              Tipo 
                                            FROM   owtr T0 
                                                   INNER JOIN wtr1 t1 
                                                           ON t1.docentry = 
                                                              t0.docentry 
                                            WHERE  t1.whscode = 'MQHEOBRA' 
                                                   AND T0.u_mq_rise = @FolioOri 
                                                   AND t1.ocrcode2 IS NOT NULL 
                                                   AND t1.quantity > 0 
                                                   AND t0.canceled = 'N' 
                                            GROUP  BY t1.ocrcode2) A0 
                                           INNER JOIN owtr A1 
                                                   ON a1.docentry = A0.folio) B2 
                                ON B2.ocrcode2 = b0.ocrcode2 
                                   AND b0.tipo = 'C' 
                                   AND b2.docdate <= b0.docdate 
                  WHERE  ( b1.docentry IS NULL 
                           AND b0.tipo = 'A' ) 
                          OR ( b2.docentry IS NULL AND b2.docentry IS NOT NULL
                               AND b0.tipo = 'C' )) W1 
              ON w1.docentry = w0.docentry 
                 AND w1.ocrcode2 = w0.ocrcode2 
                 AND WW.u_prccode IS NULL 
       LEFT JOIN wtr1 w2 
              ON w2.docentry = w0.docentry 
                 AND w0.tipo = 'A' 
                 AND w2.ocrcode2 = w0.ocrcode2 
       LEFT JOIN pch1 w3 
              ON w3.docentry = w0.docentry 
                 AND w0.tipo = 'C' 
                 AND w3.ocrcode2 = w0.ocrcode2 
       INNER JOIN oitm w4 
               ON w4.itemcode = w2.itemcode 
                   OR w4.itemcode = w3.itemcode 
       INNER JOIN oitm w5 
               ON w5.itemcode = w0.ocrcode2 
       LEFT JOIN [@ug_glo_equipo] w6 
              ON w6.code = w4.u_glo_equipo 
                  OR w6.code = w5.u_glo_equipo 
	   LEFT JOIN [@UG_MQ_RIPO] W7
			  ON W7.U_PrcCode = W0.OcrCode2 AND W7.U_IdRise = @FolioOri
WHERE  W1.docentry IS NULL 
GROUP  BY w0.ocrcode2, 
          w0.tipo, 
          w6.u_typeeq,
		  w7.Code