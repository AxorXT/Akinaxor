[System.Serializable]
public class AVLNode
{
    public string data; // Pregunta o respuesta
    public AVLNode left; // Subarbol izquierdo
    public AVLNode right; // Subarbol derecho
    public int height; // Altura del nodo

    public AVLNode(string data)
    {
        this.data = data;
        this.height = 1;
    }
}
