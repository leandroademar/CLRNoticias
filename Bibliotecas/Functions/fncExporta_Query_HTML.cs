using System.Data;
using System.Data.SqlTypes;
using Bibliotecas.Model;
using Microsoft.SqlServer.Server;

public partial class UserDefinedFunctions
{
    [Microsoft.SqlServer.Server.SqlFunction(
        DataAccess = DataAccessKind.Read,
        SystemDataAccess = SystemDataAccessKind.Read
    )]
    public static SqlString fncExporta_Query_HTML(SqlString Ds_Query, SqlString Ds_Titulo, SqlInt32 Fl_Estilo, SqlBoolean Fl_Html_Completo)
    {

        if (Ds_Query.IsNull)
            return SqlString.Null;


        var titulo = Ds_Titulo.IsNull ? "" : Ds_Titulo.Value;


        if (Utils.QueryPerigosa(Ds_Query.Value))
            return "Query perigosa";


        var estilo = 1;

        if (!Fl_Estilo.IsNull)
            estilo = Fl_Estilo.Value;


        var servidor = new ServidorAtual().NomeServidor;

        using (var dados = Utils.ExecutaQueryRetornaDataTable(servidor, Ds_Query.Value))
        {

            var retorno = criaHtmlCabecalho(estilo, Fl_Html_Completo.Value);


            retorno += @"
        <table>";


            if (titulo.Length > 0)
            {

                retorno += @"
            <tbody>

                <tr>
                    <th colspan='" + dados.Columns.Count + @"'>" + titulo + @"</th>
                </tr>

                <tr class='subtitulo'>";


                for (var i = 0; i < dados.Columns.Count; i++)
                {
                    retorno += @"
                    <td>" + dados.Columns[i].ColumnName + "</td>";
                }

                retorno += @"
                </tr>";


            }
            else
            {


                retorno += @"
            <thead>
                <tr>";


                for (var i = 0; i < dados.Columns.Count; i++)
                {
                    retorno += @"
                    <th>" + dados.Columns[i].ColumnName + "</th>";
                }


                retorno += @"
                </tr>
            </thead>

            <tbody>";


            }




            foreach (DataRow linha in dados.Rows)
            {

                retorno += @"
                <tr>";


                foreach (DataColumn coluna in dados.Columns)
                {

                    retorno += @"
                    <td>" + linha[coluna.ColumnName] + "</td>";

                }


                retorno += @"
                </tr>";


            }


            retorno += @"
            </tbody>
        </table>";


            retorno += criaHtmlRodape(Fl_Html_Completo.Value);


            return retorno;

        }

    }


    private static string aplicaEstilo(int estilo)
    {

        var servidor = new ServidorAtual().NomeServidor;
        var dsQuery = "SELECT Ds_CSS FROM dbo.HTML_Layout_CSS WHERE Id_Layout = " + estilo;
        var html = Utils.ExecutaQueryScalar(servidor, dsQuery);

        if (string.IsNullOrEmpty(html))
        {

            html = @"
	table { padding:0; border-spacing: 0; border-collapse: collapse; }
	thead { background: #00B050; border: 1px solid #ddd; }
	th { padding: 10px; font-weight: bold; border: 1px solid #000; color: #fff; }
	tr { padding: 0; }
	td { padding: 5px; border: 1px solid #cacaca; margin:0; }";

        }


        return html;

    }


    private static string criaHtmlCabecalho(int estilo, bool Fl_Html_Completo)
    {

        var retorno = "";

        if (Fl_Html_Completo)
        {

            retorno = @"<html>
    <head>
	    <title>Titulo</title>";

        }


        retorno += @"
        <style type='text/css'>";


        retorno += aplicaEstilo(estilo);


        retorno += @"
        </style>";


        if (Fl_Html_Completo)
        {

            retorno += @"
    </head>

    <body>";

        }

        return retorno;

    }


    private static string criaHtmlRodape(bool Fl_Html_Completo)
    {

        var retorno = "";

        if (Fl_Html_Completo)
        {

            retorno += @"
    </body>

</html>";

        }

        return retorno;

    }

}