<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:cfdi="http://www.sat.gob.mx/cfd/3"  xmlns:tfd="http://www.sat.gob.mx/TimbreFiscalDigital" xmlns:pago10="http://www.sat.gob.mx/Pagos" xmlns:nomina12="http://www.sat.gob.mx/nomina12">
  <xsl:output method="html" encoding="UTF-8" indent="no"/>
  <xsl:template match="/">
    <html>
      <head>
        <style>
          @import url('https://fonts.googleapis.com/css?family=Roboto');
        </style>
        <title>COMPROBANTE FISCAL DIGITAL POR INTERNET (CFDI)</title>
        <style type="text/css">
			#cfdibody {
			text-align:justify;
			font-size: 18px;
			font-weight:normal;
			background-color: #e0f7fc;
			font-family: 'Roboto', sans-serif;
			}

			h1
			{
			font-size:26px;
			color:#008faf;
			font-weight: bold;
			margin-bottom: 5px;
			text-align: center;
			}

			.cuerpo_form {
			background-color:#fff;
			width:95%;
			margin:0 auto;
			padding: 10px 10px;
			}


			.bold { font-weight:bold; color:#008faf;}

			table.gridtable {
			border-width: 1px;
			border-color: #008faf;
			border-collapse: collapse;
			width:90%;
			margin-left:auto;
			margin-right:auto;
			}
			table.gridtable td {
			border-width: 1px;
			padding: 2px;
			border-style: solid;
			border-color: #008faf;
			vertical-align: top;
			font-size: 12px;
			}
			.titulo{
			background-color:#008faf;
			color:#fff;
			font-weight:bold;
			}
			.subtitulo{
			background-color:#6a6966;
			}
			.total{
			background-color:#feb65a;
			}
		</style>
      </head>
      <body id="cfdibody">
        <xsl:for-each select="cfdi:Comprobante">

          <!--AQUI COMIENZA CFDI 3.3 -->
          <!-- ******************************************** -->
          <!-- ******************************************** -->
          <!-- ******************************************** -->
          <xsl:if test="@Version = 3.3">

            <div class="cuerpo_form">

              <table align="center" class="gridtable">
                <tr>
                  <td style="vertical-align:middle" class="datos">
                    <h1>
                      <xsl:choose>
                        <xsl:when test="//@TipoDeComprobante = 'N'">
                          Recibo de Nómina
                        </xsl:when>
                        <xsl:otherwise>
                          Factura
                        </xsl:otherwise>
                      </xsl:choose>
                    </h1>
                  </td>
                  <td style="text-align:right;" class="datos">
                    <img src="https://simplifile.mx/wp-content/uploads/2020/07/logo1.png" height="100" />
                  </td>
                </tr>
              </table>

              <table align="center" class="gridtable">
                <tr>
                  <td style="width:50%;">
                    <table style="width:100%;" class="datos">
                      <tr>
                        <td colspan="2" class="titulo">DATOS DEL EMISOR</td>
                      </tr>
                      <tr>
                        <td class="bold">RFC</td>
                        <td class="dato">
                          <xsl:value-of select="cfdi:Emisor/@Rfc"/>
                        </td>
                      </tr>
                      <tr>
                        <td class="bold">Nombre</td>
                        <td class="dato">
                          <xsl:value-of select="cfdi:Emisor/@Nombre"/>
                        </td>
                      </tr>
                      <tr>
                        <td class="bold">Régimen Fiscal</td>
                        <td class="dato">
                          <xsl:value-of select="cfdi:Emisor/@RegimenFiscal"/>
                        </td>
                      </tr>
                      <tr>
                        <td colspan="2">
                          <br/>
                        </td>
                      </tr>
                      <tr>
                        <td colspan="2" class="titulo">DATOS DEL RECEPTOR</td>
                      </tr>
                      <tr>
                        <td class="bold">RFC</td>
                        <td class="dato">
                          <xsl:value-of select="cfdi:Receptor/@Rfc"/>
                        </td>
                      </tr>
                      <tr>
                        <td class="bold">Nombre</td>
                        <td class="dato">
                          <xsl:value-of select="cfdi:Receptor/@Nombre"/>
                        </td>
                      </tr>
                      <tr>
                        <td class="bold">Uso del CFDI</td>
                        <td class="dato">
                          <xsl:value-of select="cfdi:Receptor/@UsoCFDI"/>
                        </td>
                      </tr>
                    </table>
                  </td>
                  <td style="vertical-align:top;width:50%;">
                    <table style="width:100%;" class="datos">
                      <tr>
                        <td colspan="2" class="titulo">DATOS DEL COMPROBANTE FISCAL</td>
                      </tr>
                      <tr>
                        <td class="bold">UUID</td>
                        <td class="dato">
                          <xsl:value-of select="cfdi:Complemento/tfd:TimbreFiscalDigital/@UUID"/>
                        </td>
                      </tr>
                      <tr>
                        <td class="bold">Fecha</td>
                        <td class="dato">
                          <xsl:value-of select="@Fecha"/>
                        </td>
                      </tr>
                      <tr>
                        <td class="bold">Tipo de Comprobante</td>
                        <td class="dato">
                          <xsl:value-of select="@TipoDeComprobante"/>
                        </td>
                      </tr>
                      <tr>
                        <td class="bold">Forma de pago</td>
                        <td class="dato">
                          <xsl:value-of select="@FormaPago" />
                        </td>
                      </tr>
                      <tr>
                        <td class="bold">Metodo de pago</td>
                        <td class="dato">
                          <xsl:value-of select="@MetodoPago" />
                        </td>
                      </tr>
                      <tr>
                        <td class="bold">Moneda</td>
                        <td class="dato">
                          <xsl:value-of select="@Moneda" />
                        </td>
                      </tr>
                      <tr>
                        <td class="bold">No Certificado</td>
                        <td class="dato">
                          <xsl:value-of select="@NoCertificado" />
                        </td>
                      </tr>
                    </table>
                  </td>
                </tr>
                <tr>
                  <td colspan="2">
                    <br/>

                  </td>
                </tr>

                <xsl:if test="cfdi:Complemento/nomina12:Nomina" >
                  <tr>
                    <td class="titulo" colspan="2">INFORMACIÓN GENERAL DEL RECIBO DE NÓMINA</td>
                  </tr>
                  <tr>
                    <td colspan="2">
                      <table style="width:100%;" id="conceptos" cellpadding="0" cellspacing="0">
                        <tr style="font-weight:bolder;">
                          <td class="titulo subtitulo">TIPO NOMINA</td>
                          <td class="titulo subtitulo">FECHA DE PAGO</td>
                          <td class="titulo subtitulo">PERIODO INICIAL</td>
                          <td class="titulo subtitulo">PERIODO FINAL</td>
                          <td class="titulo subtitulo">DIAS PAGADOS</td>
                          <td class="titulo subtitulo">TOTAL PERCEPCIONES</td>
                          <td class="titulo subtitulo">TOTAL DEDUCCIONES</td>
                          <td class="titulo">TOTAL PAGADO</td>
                        </tr>
                          <tr>
                            <td>
                              <xsl:value-of select="cfdi:Complemento/nomina12:Nomina/@TipoNomina"/>
                            </td>
                            <td>
                              <xsl:value-of select="cfdi:Complemento/nomina12:Nomina/@FechaPago"/>
                            </td>
                            <td>
                              <xsl:value-of select="cfdi:Complemento/nomina12:Nomina/@FechaInicialPago"/>
                            </td>
                            <td>
                              <xsl:value-of select="cfdi:Complemento/nomina12:Nomina/@FechaFinalPago"/>
                            </td>
                            <td>
                              <xsl:value-of select="cfdi:Complemento/nomina12:Nomina/@NumDiasPagados"/>
                            </td>
                            <td>
                              $<xsl:value-of select="format-number(cfdi:Complemento/nomina12:Nomina/@TotalPercepciones, '###,###.00')"/>
                            </td>
                            <td>
                              $<xsl:value-of select="format-number(cfdi:Complemento/nomina12:Nomina/@TotalDeducciones, '###,###.00')"/>
                            </td>
                            <td style="font-weight:bolder;">
                              $<xsl:value-of select="format-number(@Total, '###,###.00')"/>
                            </td>
                          </tr>
                      </table>
                    </td>
                  </tr>
                  <tr>
                  <td colspan="2">
                    <br/>

                  </td>
                </tr>
                </xsl:if>

                <xsl:if test="cfdi:Conceptos" >
                  <tr>
                    <td class="titulo" colspan="2">CONCEPTOS</td>
                  </tr>
                  <tr>
                    <td colspan="2">
                      <table style="width:100%;" id="conceptos" cellpadding="0" cellspacing="0">
                        <tr style="font-weight:bolder;">
                          <td class="titulo subtitulo">CLAVE</td>
                          <td class="titulo subtitulo">CANTIDAD</td>
                          <td class="titulo subtitulo">UNIDAD</td>
                          <td class="titulo subtitulo">DESCRIPCION</td>
                          <td class="titulo subtitulo">VALOR UNITARIO</td>
                          <td class="titulo subtitulo">DESCUENTO</td>
                          <td class="titulo subtitulo">IMPORTE</td>
                        </tr>
                        <xsl:for-each select="cfdi:Conceptos/cfdi:Concepto">
                          <tr>
                            <td>
                              <xsl:value-of select="@ClaveProdServ"/>
                            </td>
                            <td>
                              <xsl:value-of select="@Cantidad"/>
                            </td>
                            <td>
                              <xsl:value-of select="@ClaveUnidad"/>
                            </td>
                            <td>
                              <xsl:value-of select="@Descripcion"/>
                              <xsl:if test="cfdi:InformacionAduanera" >
                                <br/>
                                <p style="text-align:left;font-size:0.8em;">
                                  Inf. Aduanera<br/>
                                  - Numero: <xsl:value-of select="cfdi:InformacionAduanera/@numero"/><br/>
                                  - Fecha: <xsl:value-of select="cfdi:InformacionAduanera/@fecha"/><br/>
                                  - Aduana: <xsl:value-of select="cfdi:InformacionAduanera/@aduana"/>
                                </p>
                              </xsl:if>
                            </td>
                            <td>
                              $<xsl:value-of select="format-number(@ValorUnitario, '###,###.00')"/>
                            </td>
                            <td>
                              $<xsl:value-of select="format-number(@Descuento, '###,###.00')"/>
                            </td>
                            <td>
                              $<xsl:value-of select="format-number(@Importe, '###,###.00')"/>
                            </td>
                          </tr>
                        </xsl:for-each>
                      </table>
                    </td>
                  </tr>
                </xsl:if>

                <xsl:if test="//@TipoDeComprobante = 'N'">
                  <tr>
                    <td class="" colspan="2"><br/></td>
                  </tr>
                  <tr>
                    <td class="titulo" colspan="2">PERCEPCIONES</td>
                  </tr>
                  <tr>
                    <td colspan="2">

                      <table style="width:100%;" id="conceptos" cellpadding="0" cellspacing="0">
                        <xsl:for-each select="cfdi:Complemento/nomina12:Nomina/nomina12:Percepciones/nomina12:Percepcion">
                        <tr style="font-weight:bolder;">
                          <td class="titulo subtitulo">TIPO PERCEPCIÓN</td>
                          <td class="titulo subtitulo">CLAVE</td>
                          <td class="titulo subtitulo">CONCEPTO</td>
                          <td class="titulo subtitulo">IMPORTE GRAVADO</td>
                        </tr>
                          <tr>
                            <td>
                              <xsl:value-of select="@TipoPercepcion"/>
                            </td>
                            <td>
                              <xsl:value-of select="@Clave"/>
                            </td>
                            <td>
                              <xsl:value-of select="@Concepto"/>
                            </td>
                            <td>
                              $<xsl:value-of select="format-number(@ImporteGravado, '###,###.00')"/>
                            </td>
                          </tr>
                        </xsl:for-each>
                        <tr style="font-weight:bolder;">
                          <td colspan="2" > </td>
                          <td class="titulo">TOTAL SUELDOS</td>
                          <td class="titulo">TOTAL GRAVADO</td>
                        </tr>
                        <tr>
                          <td colspan="2" > </td>
                          <td style="font-weight:bolder;">$<xsl:value-of select="format-number(cfdi:Complemento/nomina12:Nomina/nomina12:Percepciones/@TotalSueldos, '###,###.00')"/></td>
                          <td style="font-weight:bolder;">$<xsl:value-of select="format-number(cfdi:Complemento/nomina12:Nomina/nomina12:Percepciones/@TotalGravado, '###,###.00')"/></td>
                        </tr>
                      </table>

                    </td>
                  </tr>
                  <tr>
                    <td class="" colspan="2"><br/></td>
                  </tr>
                  <tr>
                    <td class="titulo" colspan="2">DEDUCCIONES</td>
                  </tr>
                  <tr>
                    <td colspan="2">

                      <table style="width:100%;" id="conceptos" cellpadding="0" cellspacing="0">
                        <xsl:for-each select="cfdi:Complemento/nomina12:Nomina/nomina12:Deducciones/nomina12:Deduccion">
                        <tr style="font-weight:bolder;">
                          <td class="titulo subtitulo">TIPO DEDUCCIÓN</td>
                          <td class="titulo subtitulo">CLAVE</td>
                          <td class="titulo subtitulo">CONCEPTO</td>
                          <td class="titulo subtitulo">IMPORTE</td>
                        </tr>
                          <tr>
                            <td>
                              <xsl:value-of select="@TipoDeduccion"/>
                            </td>
                            <td>
                              <xsl:value-of select="@Clave"/>
                            </td>
                            <td>
                              <xsl:value-of select="@Concepto"/>
                            </td>
                            <td>
                              $<xsl:value-of select="format-number(@Importe, '###,###.00')"/>
                            </td>
                          </tr>
                        </xsl:for-each>
                        <tr style="font-weight:bolder;">
                          <td colspan="2" > </td>
                          <td class="titulo">TOTAL OTRAS DEDUCCIONES</td>
                          <td class="titulo">TOTAL IMPUESTOS RETENIDOS</td>
                        </tr>
                        <tr>
                          <td colspan="2"> </td>
                          <td style="font-weight:bolder;">$<xsl:value-of select="format-number(cfdi:Complemento/nomina12:Nomina/nomina12:Deducciones/@TotalOtrasDeducciones, '###,###.00')"/></td>
                          <td style="font-weight:bolder;">$<xsl:value-of select="format-number(cfdi:Complemento/nomina12:Nomina/nomina12:Deducciones/@TotalImpuestosRetenidos, '###,###.00')"/></td>
                        </tr>
                      </table>

                    </td>
                  </tr>
                </xsl:if>

              </table>

            </div>
          </xsl:if>
          <!--AQUI FINALIZA CFDI 3.3 -->
          <!-- ******************************************** -->
          <!-- ******************************************** -->
          <!-- ******************************************** -->

        </xsl:for-each>
      </body>
    </html>
  </xsl:template>
</xsl:stylesheet>