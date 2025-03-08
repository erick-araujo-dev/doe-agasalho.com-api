using DoeAgasalhoApiV2._0.Exceptions;
using DoeAgasalhoApiV2._0.Services.Interface;
using System.Text;

namespace DoeAgasalhoApiV2._0.Services
{
    public class EnderecoService : IEnderecoService
    {
        public void ValidateAddress(int numero, string logradouro, string complemento, string bairro, string cidade, string estado, string cep)
        {
            if (numero < 0)
            {
                throw new ArgumentException("O número do endereço deve ser um valor positivo.");
            }

            if (string.IsNullOrWhiteSpace(logradouro) || logradouro.Length > 50)
            {
                throw new ArgumentException("O logradouro do endereço deve ser preenchido e ter no máximo 50 caracteres.");
            }

            if (!string.IsNullOrWhiteSpace(complemento) && complemento.Length > 10)
            {
                throw new ArgumentException("O complemento do endereço deve ter no máximo 10 caracteres.");
            }

            if (string.IsNullOrWhiteSpace(bairro) || bairro.Length > 50)
            {
                throw new ArgumentException("O bairro do endereço deve ser preenchido e ter no máximo 50 caracteres.");
            }

            if (string.IsNullOrWhiteSpace(cidade) || cidade.Length > 50)
            {
                throw new ArgumentException("A cidade do endereço deve ser preenchida e ter no máximo 50 caracteres.");
            }

            if (string.IsNullOrWhiteSpace(estado))
            {
                throw new ArgumentException("O estado do endereço deve ser preenchido.");
            }

            if (string.IsNullOrWhiteSpace(cep))
            {
                throw new ArgumentException("O CEP do endereço deve ser preenchido.");
            }
        }


        public string AbbreviateState(string estado)
        {
            if (estado.Trim().Length >= 3)
            {
                switch (estado?.ToUpper())
                {
                    case "ACRE":
                        return "AC";
                    case "ALAGOAS":
                        return "AL";
                    case "AMAPÁ":
                        return "AP";
                    case "AMAZONAS":
                        return "AM";
                    case "BAHIA":
                        return "BA";
                    case "CEARÁ":
                        return "CE";
                    case "DISTRITO FEDERAL":
                        return "DF";
                    case "ESPÍRITO SANTO":
                        return "ES";
                    case "GOIAS":
                        return "GO";
                    case "MARANHAO":
                        return "MA";
                    case "MATO GROSSO":
                        return "MT";
                    case "MATO GROSSO DO SUL":
                        return "MS";
                    case "MINAS GERAIS":
                        return "MG";
                    case "PARÁ":
                        return "PA";
                    case "PARAÍBA":
                        return "PB";
                    case "PARANÁ":
                        return "PR";
                    case "PERNAMBUCO":
                        return "PE";
                    case "PIAUÍ":
                        return "PI";
                    case "RIO DE JANEIRO":
                        return "RJ";
                    case "RIO GRANDE DO NORTE":
                        return "RN";
                    case "RIO GRANDE DO SUL":
                        return "RS";
                    case "RONDÔNIA":
                        return "RO";
                    case "RORAIMA":
                        return "RR";
                    case "SANTA CATARINA":
                        return "SC";
                    case "SÃO PAULO":
                        return "SP";
                    case "SERGIPE":
                        return "SE";
                    case "TOCANTINS":
                        return "TO";
                    default:
                        return estado;
                }
            }
            return estado.Trim().ToUpper();
        }

        public void ValidateStateName(string estado)
        {
            var siglasValidas = new List<string> { "AC", "AL", "AP", "AM", "BA", "CE", "DF", "ES", "GO", "MA", "MT", "MS", "MG", "PA", "PB", "PR", "PE", "PI", "RJ", "RN", "RS", "RO", "RR", "SC", "SP", "SE", "TO" };
            if (!siglasValidas.Contains(estado.ToUpper()))
            {
                throw new BusinessOperationException("Nome de estado inválido, digite um nome válido.");
            }
        }


        public string FormatZipCode(string cep)
        {
            if (!string.IsNullOrWhiteSpace(cep) && cep.Length == 8)
            {
                cep = cep.Insert(5, "-");
            }
            else
            {
                throw new ArgumentException("Digite o cep completo com 8 números sem o traço ('-')");
            }

            return cep;
        }

    }
}