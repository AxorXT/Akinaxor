using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Akinaxor : MonoBehaviour
{
    public TextMeshProUGUI questionText;

    private AVLTree tree;
    private AVLNode currentNode;

    void Start()
    {
        tree = new AVLTree();

        // Crear preguntas y respuestas iniciales
        tree.root = tree.Insert(tree.root, "¿Tiene pelo?");
        tree.root = tree.Insert(tree.root, "¿Es un mamífero?");
        tree.root = tree.Insert(tree.root, "Es un perro.");
        tree.root = tree.Insert(tree.root, "Es un reptil.");

        currentNode = tree.root;
        ShowCurrentQuestion();
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
            Debug.Log("¡Respuesta encontrada: " + currentNode.data + "!");
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
            Debug.Log("¡Respuesta encontrada: " + currentNode.data + "!");
        }
    }

    private void ShowCurrentQuestion()
    {
        if (currentNode.left == null && currentNode.right == null)
        {
            Debug.Log("Respuesta: " + currentNode.data);
        }
        else
        {
            Debug.Log("Pregunta: " + currentNode.data);
        }
    }
}
