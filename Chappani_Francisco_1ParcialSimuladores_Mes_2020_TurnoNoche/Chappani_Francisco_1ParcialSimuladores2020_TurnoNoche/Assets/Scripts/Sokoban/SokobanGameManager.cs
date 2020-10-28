using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using System.Collections;

public class SokobanGameManager : MonoBehaviour
{
    Nivel nivel, nivelAux;
    GameObject casillero, casilleroTarget, pared, jugador, bloque;
    List<Vector2> posOcupadasEsperadasCasillerosTarget;
    //List<Tablero> anterioresTableros = new List<Tablero>();
    Queue<Tablero> queuetableros = new Queue<Tablero>();
    Stack<Tablero> pilaTablerosAnteriores = new Stack<Tablero>();
    List<Vector2> casillerosTarget = new List<Vector2>();
    Light luz;


    string orientacionJugador;
    string nombreNivelActual = "Nivel1";
    bool gameOver = false;
    bool estoyDeshaciendo = false;

    private void Awake()
    {
        
    }

    private void Start()
    {
        casillero = SokobanLevelManager.instancia.dameLstPrefabsSokoban().Find(x => x.name == "Casillero");
        casilleroTarget = SokobanLevelManager.instancia.dameLstPrefabsSokoban().Find(x => x.name == "CasilleroTarget");
        pared = SokobanLevelManager.instancia.dameLstPrefabsSokoban().Find(x => x.name == "Pared");
        jugador = SokobanLevelManager.instancia.dameLstPrefabsSokoban().Find(x => x.name == "Jugador");
        bloque = SokobanLevelManager.instancia.dameLstPrefabsSokoban().Find(x => x.name == "Bloque");
        CargarNivel(nombreNivelActual);
        casillerosTarget = nivel.Tablero.damePosicionesObjetos("CasilleroTarget");
        gameOver = false;
        luz = jugador.gameObject.GetComponent<Light>();
    }

    private void CargarNivel(string nombre)
    {
        nivel = SokobanLevelManager.instancia.dameNivel(nombre);
        posOcupadasEsperadasCasillerosTarget = nivel.Tablero.damePosicionesObjetos("CasilleroTarget");
        InstanciadorPrefabs.instancia.graficarCasilleros(nivel.Tablero, casillero);
        InstanciadorPrefabs.instancia.graficarCasillerosTarget(nivel.Tablero, casilleroTarget);
        InstanciadorPrefabs.instancia.graficarObjetosTablero(nivel.Tablero, SokobanLevelManager.instancia.dameLstPrefabsSokoban());
    }

    private void Update()
    {
        if(!gameOver)
        {
            if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                orientacionJugador = "derecha";
                mover();
            }
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                orientacionJugador = "arriba";
                mover();
            }
            if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                orientacionJugador = "abajo";
                mover();
            }
            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                orientacionJugador = "izquierda";
                mover();
            }
            if (Input.GetKeyDown(KeyCode.Z))
            {
                estoyDeshaciendo = true;
                mover();
            }
        }

    }

    private void mover()
    {
        if (estoyDeshaciendo == false)
        {
            Tablero tablAux = new Tablero(nivel.Tablero.casilleros.GetLength(0), nivel.Tablero.casilleros.GetLength(1));
            tablAux.setearObjetos(casillero, nivel.Tablero.damePosicionesObjetos("Casillero"));
            tablAux.setearObjetos(casilleroTarget, nivel.Tablero.damePosicionesObjetos("CasilleroTarget"));
            tablAux.setearObjetos(bloque, nivel.Tablero.damePosicionesObjetos("Bloque"));
            tablAux.setearObjetos(pared, nivel.Tablero.damePosicionesObjetos("Pared"));
            tablAux.setearObjetos(jugador, nivel.Tablero.damePosicionesObjetos("Jugador"));

            //pilaTablerosAnteriores.Push(tablAux);

            Vector2 posicionJugador = new Vector2(nivel.Tablero.damePosicionObjeto("Jugador").x, nivel.Tablero.damePosicionObjeto("Jugador").y);
            GameObject objProximo, objProximoProximo;
            objProximo = nivel.Tablero.dameObjeto(posicionJugador, orientacionJugador, 1);
            objProximoProximo = nivel.Tablero.dameObjeto(posicionJugador, orientacionJugador, 2);

            if(orientacionJugador == "izquierda" || orientacionJugador == "abajo")
            {
                objProximo = nivel.Tablero.dameObjeto(posicionJugador, orientacionJugador, -1);
                objProximoProximo = nivel.Tablero.dameObjeto(posicionJugador, orientacionJugador, -2);
            }

            if (objProximo != null && objProximo.CompareTag("casillero"))
            {
                if(orientacionJugador == "izquierda" || orientacionJugador == "abajo")
                {
                    nivel.Tablero.setearObjeto(casillero, posicionJugador);
                    nivel.Tablero.setearObjeto(jugador, posicionJugador, orientacionJugador, -1);
                } 
                else if(orientacionJugador == "arriba" || orientacionJugador == "derecha")
                {
                    nivel.Tablero.setearObjeto(casillero, posicionJugador);
                    nivel.Tablero.setearObjeto(jugador, posicionJugador, orientacionJugador, 1);
                }

            }
            else
            {
                if (objProximo.CompareTag("bloque") && objProximoProximo.CompareTag("bloque"))
                {

                }                                              
                else if(objProximo != null && objProximo.CompareTag("bloque") && objProximoProximo != null)
                {
                    if (orientacionJugador == "izquierda" || orientacionJugador == "abajo")
                    {
                        nivel.Tablero.setearObjeto(casillero, posicionJugador);
                        nivel.Tablero.setearObjeto(jugador, posicionJugador, orientacionJugador, -1);
                        nivel.Tablero.setearObjeto(bloque, posicionJugador, orientacionJugador, -2);
                    }
                    else if (orientacionJugador == "arriba" || orientacionJugador == "derecha")
                    {
                        nivel.Tablero.setearObjeto(casillero, posicionJugador);
                        nivel.Tablero.setearObjeto(jugador, posicionJugador, orientacionJugador, 1);
                        nivel.Tablero.setearObjeto(bloque, posicionJugador, orientacionJugador, 2);
                    }
                }
            }

            //InstanciadorPrefabs.instancia.graficarObjetosTablero(nivel.Tablero, SokobanLevelManager.instancia.dameLstPrefabsSokoban());
            queuetableros.Enqueue(nivel.Tablero);
            //pilaTablerosAnteriores.Push(nivel.Tablero);

            Debug.Log(queuetableros.Count);

            if (ChequearVictoria(nivel.Tablero))
            {
                Debug.Log("Gané");
                gameOver = true;
            }
        }
        else
        {
            if (pilaTablerosAnteriores.Count > 0)
            {
                Tablero ultimoTablero = pilaTablerosAnteriores.First<Tablero>();
                nivel.Tablero = ultimoTablero;

                InstanciadorPrefabs.instancia.graficarObjetosTablero(pilaTablerosAnteriores.Pop(), SokobanLevelManager.instancia.dameLstPrefabsSokoban());
            }
            estoyDeshaciendo = false;
        }
    }

    private bool SonIgualesLosVectores(Vector2 v1, Vector2 v2)
    {
        return (v1.x == v2.x && v1.y == v2.y);
    }

    private bool ChequearVictoria(Tablero tablero)
    {
        List<Vector2> bloques = tablero.damePosicionesObjetos("Bloque");

        int onTarget = 0;

        foreach(Vector2 cas in casillerosTarget)
        {
            foreach(Vector2 blk in bloques)
            {
                if(SonIgualesLosVectores(cas, blk))
                {
                    onTarget++;
                    Debug.Log(onTarget);
                }
            }
        }

        if (onTarget == casillerosTarget.Count)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void GraficarProximoTablero()
    {
        if (queuetableros.Count > 0)
        {
            InstanciadorPrefabs.instancia.graficarObjetosTablero(queuetableros.Dequeue(), SokobanLevelManager.instancia.dameLstPrefabsSokoban());
            
        }
    }
    public void PrenderLuz()
    {
        luz.enabled=true;
        queuetableros.Enqueue(nivel.Tablero);
        luz.enabled=false;
    }
}

