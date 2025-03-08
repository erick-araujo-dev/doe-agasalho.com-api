namespace DoeAgasalhoApiV2._0.Services.Interface
{
    public interface IEnderecoService
    {
        void ValidateAddress(int numero, string logradouro, string complemento, string bairro, string cidade, string estado, string cep);

        string AbbreviateState(string estado);

        void ValidateStateName(string estado);

        string FormatZipCode(string cep);
    }
}
