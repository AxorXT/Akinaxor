[System.Serializable]
public class AVLNode
{
    public string data; // Pregunta o respuesta
    public AVLNode left; // Sub�rbol izquierdo
    public AVLNode right; // Sub�rbol derecho
    public int height; // Altura del nodo

    public AVLNode(string data)
    {
        this.data = data;
        this.height = 1;
    }
}
