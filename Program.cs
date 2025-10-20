using System;
using System.Runtime.CompilerServices;
using System.IO;

class Plato
{
    private static int contador = 0;
    public int id { private set; get; }
    private string nombre;
    private string descripcion;
    private double precio;

    public Plato(string nombre = "", string descripcion = "", double precio = 0, bool contar = true)
    {
        if(contar){
            contador++;
        }
        
        this.id = contador;
        this.nombre = nombre;
        this.descripcion = descripcion;
        this.precio = precio;
    }

    public override string ToString()
    {
        return $"🆔 ID: {id}\n🍽️  Nombre: {nombre}\n📝 Descripción: {descripcion}\n💰 Precio: ${precio}";
    }

    public Plato setNombre(string nombre)
    {

        this.nombre = nombre.Trim() != "" ? nombre : "Plato sin nombre";
        return this;
    }
    public Plato setDescripcion(string descripcion)
    {

        this.descripcion =  descripcion.Trim() != "" ? descripcion : "No tiene descripcion";
        return this;
    }
    public Plato setPrecio(double precio)
    {
        this.precio = precio;
        return this;
    }
    public string getNombre() => this.nombre;
    public string getDescripcion() => this.descripcion;
    public double getPrecio() => this.precio;
}

class Nodo
{
    private static int contador_nodo = 0;
    public int id { private set; get; }
    public Plato valor;
    public Nodo siguiente;

    public Nodo(Plato obj)
    {
        contador_nodo++;
        this.id = contador_nodo;
        this.valor = obj;
        this.siguiente = null;
    }
}

class LinkedList
{
    public Nodo nodo_prin;

    public Nodo ultimo_nodo(Nodo nodo_act)
    {
        if (nodo_act.siguiente != null)
            return ultimo_nodo(nodo_act.siguiente);
        else
            return nodo_act;
    }

    public void agregar_nodo(Nodo nodo_agr, Nodo nodo_act = null)
    {
        if (this.nodo_prin == null)
        {
            this.nodo_prin = nodo_agr;
            return;
        }
        else if (this.nodo_prin.siguiente == null)
        {
            this.nodo_prin.siguiente = nodo_agr;
            return;
        }

        Nodo nodo_ult = ultimo_nodo(this.nodo_prin);
        nodo_ult.siguiente = nodo_agr;
    }

    public Nodo get_nodo(int id, Nodo nodo_ite = null)
    {
        if (nodo_ite == null)
            nodo_ite = this.nodo_prin;

        if (nodo_ite.id == id)
            return nodo_ite;

        if (nodo_ite.siguiente != null)
            return get_nodo(id, nodo_ite.siguiente);

        return null;
    }

    public void eliminar_nodo(int id, Nodo nodo_act = null, Nodo nodo_ant = null)
    {
        if (nodo_act == null)
            nodo_act = this.nodo_prin;

        if (nodo_act.id == id)
        {
            if (nodo_ant == null)
                this.nodo_prin = nodo_act.siguiente;
            else
                nodo_ant.siguiente = nodo_act.siguiente;
            return;
        }
        else
        {
            if (nodo_act.siguiente != null)
                eliminar_nodo(id, nodo_act.siguiente, nodo_act);
            else
                Console.WriteLine($"\n⚠️  No se encontró ningún nodo con ID {id}.");
        }
    }

    public override string ToString()
    {
        List<string> platos = new List<string>();
        Nodo nodo_act = this.nodo_prin;

        if (nodo_act == null)
            return "\n⚠️  La carta del restaurante está vacía.\n";

        while (nodo_act != null)
        {
            platos.Add(nodo_act.valor.ToString());
            nodo_act = nodo_act.siguiente;
        }

        return "\n================= 📜 CARTA DEL RESTAURANTE =================\n"
             + string.Join("\n------------------------------------------------------------\n", platos)
             + "\n============================================================\n";
    }
}

class Program
{
    // Métodos para manejar errores

    public static string LeerString(string mensaje){

        string texto;

        while(true){
            Console.WriteLine(mensaje);

            texto = Console.ReadLine();

            if(texto == ""){
                Console.WriteLine("❌ Error: el texto no puede estar vacío. Intente nuevamente.\n");
            }
            else{
                return texto;
            }
                
        }
    }
    public static double LeerDouble(string mensaje)
    {
        double valor;
        while (true)
        {
            Console.Write(mensaje);
            try
            {
                valor = Double.Parse(Console.ReadLine());
                return valor;
            }
            catch (FormatException)
            {
                Console.WriteLine("❌ Error: formato inválido. Ingrese un número válido.\n");
            }
        }
    }

    public static int LeerInt(string mensaje)
    {
        int valor;
        while (true)
        {
            Console.Write(mensaje);
            try
            {
                valor = int.Parse(Console.ReadLine());
                return valor;
            }
            catch (FormatException)
            {
                Console.WriteLine("❌ Error: formato inválido. Ingrese un número entero válido.\n");
            }
        }
    }

    public static LinkedList obtenerDatos(){
        LinkedList list = new LinkedList();
        string ruta = "Carta.txt";

        using(StreamReader sr = new StreamReader(ruta)){
            string linea;
            Console.WriteLine("hola");
            //Avanza por cada linea escrita del bloc de notas
            
            
            string nombre = "";
            string descripcion = "";
            double precio = 0;

            while((linea = sr.ReadLine()) != null){
                
                Console.WriteLine(linea);

                
                if(!string.IsNullOrWhiteSpace(linea)){
                   
                    
                    string[] partes = linea.Split(":");

                    
                    string key = partes[0].Trim();
                    string value = partes[1].Trim();

                   

                    if(key.ToLower()=="nombre"){
                        
                        nombre = value;
                    }
                    if(key.ToLower()=="descripcion"){
                        descripcion = value;
                    }
                    if(key.ToLower()=="precio"){
                        precio = double.Parse(value);
                    }

                    if(nombre != "" && descripcion != "" && precio != 0){
                        Plato plato = new Plato(nombre, descripcion, precio);
                        Nodo nodo = new Nodo(plato);

                        list.agregar_nodo(nodo);

                        nombre = "";
                        descripcion = "";
                        precio = 0;
                    }

                }
                
            }
            
        }
        return list;
    }

    //Guardado de datos
    public static void guardarDatos(LinkedList lista){
        string ruta = "Carta.txt";

        using(StreamWriter sr = new StreamWriter(ruta)){
            
            void guardarNodos(Nodo nodo_guardar){
                Plato plato = nodo_guardar.valor;
                Nodo plato_siguiente = nodo_guardar.siguiente !=null ? nodo_guardar.siguiente : null;

                string nombre = plato.getNombre();
                string descripcion = plato.getDescripcion();
                double precio = plato.getPrecio();

                //Guardar los atributos
                sr.WriteLine($"Nombre: {nombre}");
                sr.WriteLine($"Descripcion: {descripcion}");
                sr.WriteLine($"Precio: {precio}");

                //Espaciado para el siguiente plato
                sr.WriteLine("");

                if(plato_siguiente !=null){
                    guardarNodos(plato_siguiente);
                }
                
            }
            if(lista.nodo_prin==null){
                Console.WriteLine("No hay nodos a setear");
            }   
            else{
                Nodo nodo_prin = lista.nodo_prin;

                guardarNodos(nodo_prin);
            } 
            
        }
    }

    //Inicio del algoritmo
    static void Main(string[] args)
    {
        //Verificar si archivo existe
        LinkedList lista;
        string ruta = Path.Combine(Directory.GetCurrentDirectory(), "Carta.txt");
        if(File.Exists(ruta)){
            lista = obtenerDatos();
        }
        else{
            lista = new LinkedList();
        }
             

        while (true)
        {
            Console.WriteLine("\n============================================================");
            Console.WriteLine("🍽️  ADMINISTRACIÓN DE LA CARTA DEL RESTAURANTE");
            Console.WriteLine("============================================================");
            Console.WriteLine("1️⃣  Agregar un nuevo plato");
            Console.WriteLine("2️⃣  Editar un plato");
            Console.WriteLine("3️⃣  Eliminar un plato");
            Console.WriteLine("4️⃣  Ver la carta del restaurante");
            Console.WriteLine("5️⃣  Buscar un plato por ID");
            Console.WriteLine("6️⃣  Salir");
            Console.WriteLine("------------------------------------------------------------");
            Console.Write("👉  Ingrese una opción: ");

            string opcion = Console.ReadLine();
            Console.WriteLine();

            switch (opcion)
            {
                case "1":
                    Console.WriteLine("\n================= ➕ NUEVO PLATO =================");
                    string nombre = LeerString("🍽️  Nombre del plato: ");

                    string descripcion = LeerString("📝 Descripción del plato: ");

                    double precio = LeerDouble("💰 Precio: $");

                    Plato plato1 = new Plato(nombre, descripcion, precio);
                    Nodo nodo1 = new Nodo(plato1);
                    lista.agregar_nodo(nodo1);

                    Console.WriteLine("\n✅ Plato agregado exitosamente.");
                    break;

                case "2":
                    Console.WriteLine("\n================= ✏️ EDITAR PLATO =================");
                    int id_editar = LeerInt("🔍 Ingrese el ID del plato a editar: ");
                    Nodo nodo_editar = lista.get_nodo(id_editar);

                    if (nodo_editar == null)
                    {
                        Console.WriteLine("\n⚠️  Plato no encontrado.");
                        break;
                    }

                    Console.WriteLine($"\n✏️  Editando el plato '{nodo_editar.valor.getNombre()}'");
                    nodo_editar.valor.setNombre(LeerString($"🍽️  Nuevo nombre (actual: {nodo_editar.valor.getNombre()}): "));

                    Console.WriteLine();
                    nodo_editar.valor.setDescripcion(LeerString($"📝 Nueva descripción (actual: {nodo_editar.valor.getDescripcion()}): "));

                    double nuevo_precio = LeerDouble($"💰 Nuevo precio (actual: ${nodo_editar.valor.getPrecio()}): $");
                    nodo_editar.valor.setPrecio(nuevo_precio);

                    Console.WriteLine("\n✅ Plato editado exitosamente.");
                    Console.WriteLine(nodo_editar.valor);
                    break;

                case "3":
                    Console.WriteLine("\n================= ❌ ELIMINAR PLATO =================");
                    int id_eliminar = LeerInt("🗑️  Ingrese el ID del plato a eliminar: ");
                    lista.eliminar_nodo(id_eliminar);
                    Console.WriteLine("\n✅ Plato eliminado exitosamente.");
                    break;

                case "4":
                    Console.WriteLine(lista);
                    break;

                case "5":
                    Console.WriteLine("\n================= 🔎 BUSCAR PLATO =================");
                    int id_buscar = LeerInt("🔍 Ingrese el ID del plato a buscar: ");
                    Nodo nodo_encontrado = lista.get_nodo(id_buscar);
                    Console.WriteLine("\n" + (nodo_encontrado != null ? nodo_encontrado.valor.ToString() : "⚠️  Plato no encontrado."));
                    break;

                case "6":
                    guardarDatos(lista);
                    Console.WriteLine("\n👋 Gracias por usar la administración de la carta. ¡Hasta luego!");
                    return;

                default:
                    Console.WriteLine("\n⚠️  Opción no válida. Intente nuevamente.");
                    break;
            }

            Console.WriteLine("\nPresione ENTER para continuar...");
            Console.ReadLine();
            Console.Clear();
        }
    }
}
