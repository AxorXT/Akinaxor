using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.IO;

public class Akinaxor : MonoBehaviour
{
    public TextMeshProUGUI questionText; // Campo de texto para mostrar preguntas
    public TMP_InputField inputFieldQuestion; // Campo para escribir nuevas preguntas
    public TMP_InputField inputFieldAnswer; // Campo para escribir nuevas respuestas
    public GameObject inputPanel; // Panel para expandir el árbol

    private AVLTree tree;
    private AVLNode currentNode;
    private string filePath;

    void Start()
    {
        filePath = Application.persistentDataPath + "/treeData.txt";

        tree = new AVLTree();

        if (File.Exists(filePath))
        {
            LoadTree();
        }
        else
        {
            // Crear árbol inicial
            tree.root = tree.Insert(tree.root, "¿Tiene pelo?");
            tree.root = tree.Insert(tree.root, "¿Es un mamífero?");
            tree.root = tree.Insert(tree.root, "Es un perro.");
            tree.root = tree.Insert(tree.root, "Es un reptil.");
        }

        currentNode = tree.root;
        inputPanel.SetActive(false); // Ocultar panel de entrada al inicio
        ShowCurrentQuestion();
    }

    void OnApplicationQuit()
    {
        SaveTree();
    }

    public void AnswerYes()
    {
        if (currentNode.left != null)
        {
            currentNode = currentNode.left;
            ShowCurrentQuestion();
        }
        else
        {
            questionText.text = "¡Respuesta encontrada: " + currentNode.data + "!";
            ShowExpansionPanel(); // Ofrecer al jugador la oportunidad de expandir el árbol
        }
    }

    public void AnswerNo()
    {
        if (currentNode.right != null)
        {
            currentNode = currentNode.right;
            ShowCurrentQuestion();
        }
        else
        {
            questionText.text = "¡Respuesta encontrada: " + currentNode.data + "!";
            ShowExpansionPanel(); // Ofrecer al jugador la oportunidad de expandir el árbol
        }
    }

    private void ShowCurrentQuestion()
    {
        if (currentNode.left == null && currentNode.right == null)
        {
            questionText.text = "Respuesta: " + currentNode.data;
        }
        else
        {
            questionText.text = "Pregunta: " + currentNode.data;
        }
    }

    private void ShowExpansionPanel()
    {
        inputPanel.SetActive(true); // Mostrar panel de entrada
    }

    public void AddNewQuestionAndAnswer()
    {
        // Obtener datos del jugador
        string newQuestion = inputFieldQuestion.text;
        string newAnswer = inputFieldAnswer.text;

        if (!string.IsNullOrEmpty(newQuestion) && !string.IsNullOrEmpty(newAnswer))
        {
            // Crear nuevos nodos
            string oldAnswer = currentNode.data;
            currentNode.data = newQuestion;

            currentNode.left = new AVLNode(newAnswer); // Respuesta positiva
            currentNode.right = new AVLNode(oldAnswer); // Respuesta negativa

            questionText.text = "¡Nuevo conocimiento añadido! Gracias por mejorar el juego.";
            SaveTree(); // Guardar cambios
            inputPanel.SetActive(false); // Ocultar panel de entrada
        }
        else
        {
            questionText.text = "Por favor, completa ambos campos para continuar.";
        }
    }

    private void SaveTree()
    {
        List<string> serializedData = new List<string>();
        SerializeTree(tree.root, serializedData);

        File.WriteAllLines(filePath, serializedData);
        Debug.Log("Árbol guardado en: " + filePath);
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
        Debug.Log("Árbol cargado desde: " + filePath);
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
}