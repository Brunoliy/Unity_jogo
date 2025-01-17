using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using Cinemachine;
public class PauseManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private string nomeDoLevelDeJogo;
    [SerializeField] private GameObject painelPauseMenu;
    [SerializeField] private GameObject painelOpcoes;
    [SerializeField] private GameObject painelSair;
    [SerializeField] private GameObject painelObjetivo;
    [SerializeField] private GameObject painelHUD;
    [SerializeField] private GameObject painelDerrota;
    [SerializeField] private GameObject painelJogadorMorreu;
    [SerializeField] private GameObject GameManager;
    [SerializeField] private GameObject botaoRecomecarJogo;
    [SerializeField] private CinemachineTargetGroup targetGroup;
    [SerializeField] private MultiJogador jogador;
    public HUD hud;

    private void Start()
    {
        // Mostrar/Recolher o botão de recomeçar jogo com base na condição de Master Client
        if (PhotonNetwork.IsMasterClient)
        {
            botaoRecomecarJogo.SetActive(true);
        }
        else
        {
            botaoRecomecarJogo.SetActive(false);
        }
    }

    public void ComecarJogo()
    {
        Time.timeScale = 1;
        GameManager.SetActive(true);
        painelObjetivo.SetActive(false);
        painelHUD.SetActive(true);
    }

    public void Renascer()
    {
        painelJogadorMorreu.SetActive(false);
        painelHUD.SetActive(true);
        hud.AtivarVida(jogador.vidas);

        // Renasça o jogador local
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in players)
        {
            MultiJogador jogador = player.GetComponent<MultiJogador>();
            if (jogador.photonView.IsMine && jogador.Derrotado)
            {
                jogador.Reviver();
                break;
            }
        }
    }
    public void RecomecarJogo()
    {
        string nomeDoLevelDeJogo = SceneManager.GetActiveScene().name;
        painelDerrota.SetActive(false);

        if (nomeDoLevelDeJogo == "GameplayMultiplayer")
        {
            PhotonNetwork.AutomaticallySyncScene = true;
            PhotonNetwork.LoadLevel("GameplayMultiplayer");
            Time.timeScale = 1;
        }
        else
        {
            SceneManager.LoadScene(nomeDoLevelDeJogo);
            Time.timeScale = 0;
        }

    }
    public void PauseGame()
    {
        painelPauseMenu.SetActive(true);

        if (nomeDoLevelDeJogo != "GameplayMultiplayer")
        {
            Time.timeScale = 0;
        }
    }
    public void Resume()
    {
        painelPauseMenu.SetActive(false);

        if (nomeDoLevelDeJogo != "GameplayMultiplayer")
        {
            Time.timeScale = 1;
        }
    }
    public void Opcoes()
    {
        painelPauseMenu.SetActive(false);
        painelOpcoes.SetActive(true);
    }
    public void FecharOpcoes()
    {
        painelOpcoes.SetActive(false);
        painelPauseMenu.SetActive(true);
    }
    public void Sair()
    {
        painelPauseMenu.SetActive(false);
        painelSair.SetActive(true);

    }
    public void NaoSair()
    {
        painelSair.SetActive(false);
        painelPauseMenu.SetActive(true);

    }
    public void MenuInicial()
    {
        if (nomeDoLevelDeJogo == "GameplayMultiplayer")
        {
            PhotonNetwork.LeaveRoom();

        }
        SceneManager.LoadScene("Menu");
    }

    public override void OnLeftRoom()
    {
        PhotonNetwork.Disconnect();
    }


}
