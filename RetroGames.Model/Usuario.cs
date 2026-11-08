namespace RetroGames.Model.Model
{
    public class Usuario
    {
        // Chave primária: co_seq_usuario
        public int Id { get; set; }

        // Nome do usuário: no_usuario
        public string Nome { get; set; } = string.Empty;

        // E-mail do usuário: ds_email
        public string Email { get; set; } = string.Empty;

        // Hash da senha: ds_senha_hash
        public string SenhaHash { get; set; } = string.Empty;

        // Indica se o usuário está ativo: st_ativo
        public bool Ativo { get; set; }

        // Data de cadastro: dt_cadastro
        public DateTime DataCadastro { get; set; }

        // Chave estrangeira do perfil: co_perfil
        public int IdPerfil { get; set; }
    }
}