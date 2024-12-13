using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.IO;

public class Akinaxor : MonoBehaviour
{
    public TextMeshProUGUI questionText; // Texto para mostrar preguntas
    public GameObject inputPanel; // Panel para ingresar nueva informaci�n
    public TMP_InputField inputNewObject; // Campo para ingresar el nuevo objeto
    public TMP_InputField inputNewQuestion; // Campo para ingresar la nueva pregunta
    public GameObject restartButton;

    public Button btnSi;  // Bot�n de "S�"
    public Button btnNo;  // Bot�n de "No"
    public Image canvasImage; // Referencia a la imagen que se cambia en el Canvas

    public Sprite[] imagesYes;  // Arreglo de im�genes para el caso de "S�"
    public Sprite[] imagesNo;   // Arreglo de im�genes para el caso de "No"
    public Sprite[] imagesDefault;  // Arreglo de im�genes por defecto
    public Image imageCorrect; // Imagen para mostrar cuando acierte

    private int currentImageIndex = 0;  // �ndice para navegar entre las im�genes
    private AVLTree tree;
    private AVLNode currentNode;
    private string filePath;
    private bool isGamePaused = false;
    private string lastAnswer = "";  // Variable para almacenar la �ltima respuesta ("S�" o "No")

    private void Start()
    {
        filePath = Application.persistentDataPath + "/treeData.txt";
        tree = new AVLTree();

        if (File.Exists(filePath))
        {
            LoadTree();
        }
        else
        {
            tree.root = tree.Insert(tree.root, "Es un animal");
            tree.root.left = new AVLNode("un perro");
            tree.root.right = new AVLNode("una computadora");
        }

        currentNode = tree.root;
        inputPanel.SetActive(false);
        ShowCurrentQuestion();

        // Establecer la primera imagen predeterminada
        SetImageDefault();
        imageCorrect.gameObject.SetActive(false); // Aseg�rate de que la imagen de "Acierto" est� oculta al inicio
    }

    private void OnApplicationQuit()
    {
        SaveTree();
    }

    public void AnswerYes()
    {
        if (isGamePaused) return;

        lastAnswer = "Si";  // Actualizar la respuesta como "S�"

        // Cambiar imagen a la de "S�"
        currentImageIndex = (currentImageIndex + 1) % imagesYes.Length;
        canvasImage.sprite = imagesYes[currentImageIndex];

        if (currentNode.left != null)
        {
            currentNode = currentNode.left;
            ShowCurrentQuestion();
        }
        else
        {
            questionText.text = "�Es " + currentNode.data + "! �He acertado!";
            restartButton.SetActive(true);

            // Ocultar las im�genes de respuesta (Yes/No)
            canvasImage.gameObject.SetActive(false);

            // Mostrar imagen de acierto
            imageCorrect.gameObject.SetActive(true);

            // Pausar el juego (si es necesario)
            PauseGame();
        }
    }

    public void AnswerNo()
    {
        if (isGamePaused) return;

        lastAnswer = "No";  // Actualizar la respuesta como "No"

        // Cambiar imagen a la de "No"
        currentImageIndex = (currentImageIndex + 1) % imagesNo.Length;
        canvasImage.sprite = imagesNo[currentImageIndex];

        if (currentNode.right != null)
        {
            currentNode = currentNode.right;
            ShowCurrentQuestion();
        }
        else
        {
            questionText.text = "�Qu� era?";
            ShowExpansionPanel();  // Aqu� se muestra el panel de expansi�n
            restartButton.SetActive(true);

            // Pausar el juego (si es necesario)
            PauseGame();
        }
    }

    private void ShowCurrentQuestion()
    {
        if (currentNode.left == null && currentNode.right == null) // Si es una respuesta (hoja)
        {
            questionText.text = "�Es " + currentNode.data + "?";  // Muestra la respuesta sin signo de interrogaci�n
        }
        else // Si es una pregunta
        {
            questionText.text = "�" + currentNode.data + "?"; // A�adir el signo de interrogaci�n a las preguntas
        }
    }

    private void ShowExpansionPanel()
    {
        inputPanel.SetActive(true);
        btnSi.interactable = false;
        btnNo.interactable = false;
    }

    public void AddNewQuestionAndAnswer()
    {
        string newObject = inputNewObject.text.Trim();    // Objeto nuevo introducido
        string newQuestion = inputNewQuestion.text.Trim();  // Pregunta nueva introducida

        if (!string.IsNullOrEmpty(newObject) && !string.IsNullOrEmpty(newQuestion))
        {
            // Crear los nuevos nodos
            AVLNode newNode = new AVLNode(newObject);  // Nodo del nuevo objeto
            AVLNode oldNode = new AVLNode(currentNode.data);  // Nodo antiguo con el dato actual

            // Actualizar el nodo actual con la nueva pregunta y sus respuestas
            currentNode.data = newQuestion;

            if (lastAnswer == "Si")
            {
                currentNode.left = oldNode; // La pregunta anterior queda en la rama "S�"
                currentNode.right = newNode; // La respuesta nueva queda en la rama "No"
            }
            else if (lastAnswer == "No")
            {
                currentNode.left = newNode;  // La respuesta nueva queda en la rama "S�"
                currentNode.right = oldNode; // La pregunta anterior queda en la rama "No"
            }

            // Ocultar el panel de entrada y continuar el juego
            inputPanel.SetActive(false);
            questionText.text = "�Gracias! He aprendido algo nuevo.";

            // Habilitar los botones y restablecer la imagen predeterminada
            btnSi.interactable = true;
            btnNo.interactable = true;
            SetImageDefault();
        }
        else
        {
            questionText.text = "Por favor, llena ambos campos para continuar.";
        }
    }

    private void SetImageDefault()
    {
        // Cambiar la imagen a la primera de las im�genes por defecto
        canvasImage.sprite = imagesDefault[0];
        canvasImage.gameObject.SetActive(true);
    }

    private void SaveTree()
    {
        List<string> serializedData = new List<string>();
        SerializeTree(tree.root, serializedData);
        File.WriteAllLines(filePath, serializedData);
        Debug.Log("�rbol guardado en: " + filePath);
    }

    private void SerializeTree(AVLNode node, List<string> data)
    {
        if (node == null)
        {
            data.Add("null");
            return;
        }

        data.Add(node.data);
        SerializeTree(node.left, data);
        SerializeTree(node.right, data);
    }

    private void LoadTree()
    {
        string[] lines = File.ReadAllLines(filePath);
        int index = 0;
        tree.root = DeserializeTree(lines, ref index);
        Debug.Log("�rbol cargado desde: " + filePath);
    }

    private AVLNode DeserializeTree(string[] data, ref int index)
    {
        if (index >= data.Length || data[index] == "null")
        {
            index++;
            return null;
        }

        AVLNode node = new AVLNode(data[index]);
        index++;
        node.left = DeserializeTree(data, ref index);
        node.right = DeserializeTree(data, ref index);

        return node;
    }

    public void ClearTree()
    {
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }

        tree.root = tree.Insert(null, "Es un animal");
        tree.root.left = new AVLNode("un perro");
        tree.root.right = new AVLNode("una computadora");

        currentNode = tree.root;
        ShowCurrentQuestion();
        Debug.Log("�rbol vaciado y reiniciado.");
    }

    public void RestartGame()
    {
        currentNode = tree.root;
        inputPanel.SetActive(false);
        inputNewObject.text = "";
        inputNewQuestion.text = "";
        ShowCurrentQuestion();

        btnSi.interactable = true;
        btnNo.interactable = true;

        SetImageDefault();  // Establecer imagen predeterminada
        imageCorrect.gameObject.SetActive(false);  // Ocultar la imagen de "Acierto"
        isGamePaused = false;
    }

    private void PauseGame()
    {
        isGamePaused = true;
        btnSi.interactable = false;
        btnNo.interactable = false;
    }
}