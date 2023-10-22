import './App.css';
import { useEffect, useState } from 'react';
import axios from 'axios';
import { ToastContainer, toast } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';

const apiUrl = "https://localhost:7089/api/Sale";

function App() {
  const [sales, setSales] = useState([]);
  const [sale, setSale] = useState({ cut: "", quantity: "", description: "", discount: "", price: "" });
  const [isEditing, setIsEditing] = useState(false);
  const [editingSaleId, setEditingSaleId] = useState(null);

  const jwttoken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJJZCI6ImFmZDFjOTQ2LWE0YTAtNDM2NC1hYmI0LTE5Njk4OTlmMWJhYyIsInN1YiI6IkFyb25AZ21haWwuY29tIiwiZW1haWwiOiJBcm9uQGdtYWlsLmNvbSIsImp0aSI6Ijg5MzI0YmEzLTdiYmYtNDVjZC1iNjIzLTc0ZDcyNzg3M2ZhMyIsImlhdCI6MTY5Nzk0NzkxNiwibmJmIjoxNjk3OTQ3OTE2LCJleHAiOjE2OTc5NTUxMTZ9.n6A599aYjR-Ky-fBZns-HYjhhRubTE-mmGH9eAwwkgc";

  useEffect(() => {
    async function fetchSales() {
      try {
        const response = await axios.get(apiUrl, {
          headers: {
            Authorization: `Bearer ${jwttoken}`,
          },
        });
        setSales(response.data);
      } catch (error) {
        console.error("Erro ao buscar sales:", error);
      }
    }
    fetchSales();
  }, [jwttoken]);

  const handleInputChange = (event) => {
    const { name, value } = event.target;
    setSale({ ...sale, [name]: value });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();

    if (!sale.cut) {
      toast.info("Antes de prosseguirmos, preencha todos os campos!");
      return;
    }

    try {
      const saleData = {
        Cut: sale.cut,
        Quantity: parseFloat(sale.quantity),
        Description: sale.description,
        Discount: parseFloat(sale.discount),
        Price: parseFloat(sale.price),
      };

      if (isEditing) {
        // put
        await axios.put(`${apiUrl}/${editingSaleId}`, saleData, {
          headers: {
            Authorization: `Bearer ${jwttoken}`,
          },
        });
        toast.info("Sale editado com sucesso!");
      } else {
        // post
        await axios.post(apiUrl, saleData, {
          headers: {
            Authorization: `Bearer ${jwttoken}`,
          },
        });
        toast.success("Sale cadastrado com sucesso!");
      }

      const response = await axios.get(apiUrl, {
        headers: {
          Authorization: `Bearer ${jwttoken}`,
        },
      });
      setSales(response.data);
      setSale({ cut: "", quantity: "", description: "", discount: "", price: "" });

      setIsEditing(false);
      setEditingSaleId(null);
    } catch (error) {
      console.error("Erro ao cadastrar sale:", error);
      toast.error("Erro ao cadastrar sale.");
    }
  };

  const handleEdit = (id) => {
    const saleToEdit = sales.find((c) => c.id === id);
    if (saleToEdit) {
      setSale({ ...saleToEdit });
      setIsEditing(true);
      setEditingSaleId(id);
    }
  };

  const handleDelete = async (id) => {
    try {
      await axios.delete(`${apiUrl}/${id}`, {
        headers: {
          Authorization: `Bearer ${jwttoken}`,
        },
      });
      const updatedSales = sales.filter((c) => c.id !== id);
      setSales(updatedSales);
      toast.success("Sale excluído com sucesso!");
    } catch (error) {
      console.error("Erro ao excluir sale:", error);
      toast.error("Erro ao excluir sale.");
    }
  };

  return (
    <>
      <div className="sale-container">
        <h1>{isEditing ? "Editar Sale" : "Cadastro de Sale"}</h1>
        <form onSubmit={handleSubmit}>
          <div className="form-group">
            <label htmlFor="cut">Cortes:</label>
            <input type="text" id="cut" name="cut" value={sale.cut} onChange={handleInputChange} />
            <label htmlFor="quantity">Quantity:</label>
            <input type="number" id="quantity" name="quantity" value={sale.quantity} onChange={handleInputChange} />
            <label htmlFor="description">Description:</label>
            <input type="text" id="description" name="description" value={sale.description} onChange={handleInputChange} />
            <label htmlFor="discount">Discount:</label>
            <input type="number" id="discount" name="discount" value={sale.discount} onChange={handleInputChange} />
            <label htmlFor="price">Price:</label>
            <input type="number" id="price" name="price" value={sale.price} onChange={handleInputChange} />
          </div>
          <button type="submit" className="cta-button">
            {isEditing ? "Salvar Edição" : "Cadastrar Sale"}
          </button>
        </form>
        <h2>Lista de Sales</h2>
        {sales.length > 0 ? (
          <table className="sale-table">
            <thead>
              <tr>
                <th>ID</th>
                <th>Cut</th>
                <th>Quantity</th>
                <th>Description</th>
                <th>Discount</th>
                <th>Price</th>
                <th>Total Price</th>
                <th>Actions</th>
              </tr>
            </thead>
            <tbody>
              {sales.map((c) => (
                <tr key={c.id}>
                  <td>{c.id}</td>
                  <td>{c.cut}</td>
                  <td>{c.quantity}</td>
                  <td>{c.description}</td>
                  <td>{c.discount}</td>
                  <td>{c.price}</td>
                  <td>{Math.abs(c.discount - c.price).toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' })}</td>
                  <td className="cta-buttons">
                    <button className="cta-edit" onClick={() => handleEdit(c.id)}>
                      Editar
                    </button>
                    <button className="cta-delete" onClick={() => handleDelete(c.id)}>
                      Excluir
                    </button>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        ) : (
          <p>Nenhum sale encontrado.</p>
        )}
      </div>
      <ToastContainer />
    </>
  );
}

export default App;
