using System;

public class AVLTree
{
    public AVLNode root;

    private int Height(AVLNode node)
    {
        return node == null ? 0 : node.height;
    }

    private int GetBalance(AVLNode node)
    {
        return node == null ? 0 : Height(node.left) - Height(node.right);
    }

    private AVLNode RotateRight(AVLNode y)
    {
        AVLNode x = y.left;
        AVLNode T = x.right;

        x.right = y;
        y.left = T;

        y.height = Math.Max(Height(y.left), Height(y.right)) + 1;
        x.height = Math.Max(Height(x.left), Height(x.right)) + 1;

        return x;
    }

    private AVLNode RotateLeft(AVLNode x)
    {
        AVLNode y = x.right;
        AVLNode T = y.left;

        y.left = x;
        x.right = T;

        x.height = Math.Max(Height(x.left), Height(x.right)) + 1;
        y.height = Math.Max(Height(y.left), Height(y.right)) + 1;

        return y;
    }

    public AVLNode Insert(AVLNode node, string data)
    {
        if (node == null)
            return new AVLNode(data);

        if (string.Compare(data, node.data) < 0)
            node.left = Insert(node.left, data);
        else if (string.Compare(data, node.data) > 0)
            node.right = Insert(node.right, data);
        else
            return node;

        node.height = 1 + Math.Max(Height(node.left), Height(node.right));
        int balance = GetBalance(node);

        if (balance > 1 && string.Compare(data, node.left.data) < 0)
            return RotateRight(node);

        if (balance < -1 && string.Compare(data, node.right.data) > 0)
            return RotateLeft(node);

        if (balance > 1 && string.Compare(data, node.left.data) > 0)
        {
            node.left = RotateLeft(node.left);
            return RotateRight(node);
        }

        if (balance < -1 && string.Compare(data, node.right.data) < 0)
        {
            node.right = RotateRight(node.right);
            return RotateLeft(node);
        }

        return node;
    }
}