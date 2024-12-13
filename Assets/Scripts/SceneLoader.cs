using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor; // Importa el espacio de nombres de UnityEditor
#endif

public class SceneLoader : MonoBehaviour
{
    // Cargar una escena específica por nombre
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    // Salir del juego
    public void QuitGame()
    {
        #if UNITY_EDITOR
        // Detiene el modo de juego en el editor
        EditorApplication.isPlaying = false;
        #else
        // Cierra el juego en una compilación
        Application.Quit();
        #endif
    }
}
