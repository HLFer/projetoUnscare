using System;
using UnityEngine;
using System.Collections;
using System.IO.Ports;

public class Paredes : MonoBehaviour
{

    public SerialPort serial = new SerialPort("COM4", 115200);

    //Classe pública transform (padrão Unity)
    public Transform Parede_Leste;
    public Transform Parede_Norte;
    public Transform Parede_Oeste;
    public Transform Parede_Sul;
    public string inData;
    public float VelocidadeParedes = 2000;

    // Use this for initialization
    void Start()
    {
        serial.Open();
        serial.ReadTimeout = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (serial.IsOpen)
        {
            try
            {
                inData = serial.ReadLine();

                if (inData == null)
                {                                                          // Ignora se não capturar nada.
                    return;
                }
                if (inData == string.Empty)
                {                                                          // Ignora se linha estiver vazia.
                    return;
                }

                inData = inData.Trim();                                    // Remove espaços em branco .  

                if (inData.Length <= 0)
                {                                                          // Ignora se não tiver nada.
                    return;
                }

                if (inData[0] == 'B')
                {
                    Movimenta(float.Parse(inData.Substring(1)));
                }
            }
            catch (System.Exception)
            {

            }
        }
    }

    void Movimenta(float BPM)
    {
        var Movimento = Time.deltaTime * (BPM / VelocidadeParedes);//   Variável que determina a velocidade com que as paredes
                                                                   // abrem /fecham durante o tratamento - de acordo com os
                                                                   // batimentos do paciente.

        //Movimentação das paredes para o centro do cenário(Utilizando a variável deltaTime - Padrão Unity)
        this.Parede_Sul.transform.Translate(-this.Parede_Sul.transform.forward * Movimento);
        this.Parede_Norte.transform.Translate(this.Parede_Norte.transform.forward * Movimento);
        this.Parede_Leste.transform.Translate(-this.Parede_Leste.transform.right * Movimento);
        this.Parede_Oeste.transform.Translate(this.Parede_Oeste.transform.right * Movimento);
        //Movimentação das paredes para o centro do cenário

        //Condição para manter as paredes dentro do tamanho máximo do cenário
        if (this.Parede_Oeste.transform.position.x > 25)
        {
            this.Parede_Sul.transform.position = new Vector3(this.Parede_Sul.transform.position.x, this.Parede_Sul.transform.position.y, 25);
            this.Parede_Norte.transform.position = new Vector3(this.Parede_Norte.transform.position.x, this.Parede_Norte.transform.position.y, -25);
            this.Parede_Leste.transform.position = new Vector3(-25, this.Parede_Leste.transform.position.y, this.Parede_Leste.transform.position.z);
            this.Parede_Oeste.transform.position = new Vector3(25, this.Parede_Oeste.transform.position.y, this.Parede_Oeste.transform.position.z);
        }


        //condição para  que as paredes não ultrapassem o valor 5 - (obs: não passem por dentro do personagem)
        if (this.Parede_Oeste.transform.position.x < 5)
        {
            this.Parede_Sul.transform.position = new Vector3(this.Parede_Sul.transform.position.x, this.Parede_Sul.transform.position.y, 5);
            this.Parede_Norte.transform.position = new Vector3(this.Parede_Norte.transform.position.x, this.Parede_Norte.transform.position.y, -5);
            this.Parede_Leste.transform.position = new Vector3(-5, this.Parede_Leste.transform.position.y, this.Parede_Leste.transform.position.z);
            this.Parede_Oeste.transform.position = new Vector3(5, this.Parede_Oeste.transform.position.y, this.Parede_Oeste.transform.position.z);
        }
    }
}