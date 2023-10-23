import React, { useEffect, useState } from 'react';
import axios from 'axios';
import LogoutButton from '../Auth/LogoutButton';
import { useNavigate } from 'react-router-dom';
import { ToastContainer, toast } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';
import App from '../../App.css';

function SaleForm() {
  const [sales, setSales] = useState([]);
  const [sale, setSale] = useState({
    cut: '',
    quantity: '',
    description: '',
    discount: '',
    price: '',
  });
  const [isEditing, setIsEditing] = useState(false);
  const navigate = useNavigate();

  useEffect(() => {
    const token = localStorage.getItem('token');
    if (!token) {
      navigate('/login');
    } else {
      fetchSales(token);
    }
  }, [navigate]);

  const fetchSales = async (token) => {
    try {
      const response = await axios.get('https://localhost:7089/api/sale', {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      });
      setSales(response.data);
    } catch (error) {
      console.error('Erro ao buscar vendas:', error);
    }
  };

  const handleInputChange = (e) => {
    const { name, value } = e.target;
    setSale({ ...sale, [name]: value });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    const token = localStorage.getItem('token');

    if (isEditing) {
      try {
        const response = await axios.put(`https://localhost:7089/api/sale/${sale.id}`, sale, {
          headers: {
            Authorization: `Bearer ${token}`,
          },
        });

        const updatedSale = response.data;

        setSales((prevSales) =>
          prevSales.map((s) => (s.id === updatedSale.id ? updatedSale : s))
        );

        setSale({
          cut: '',
          quantity: '',
          description: '',
          discount: '',
          price: '',
        });
        setIsEditing(false);
        toast.success('Venda editada com sucesso.');
      } catch (error) {
        console.error('Erro ao editar venda:', error);
        toast.error('Erro ao editar venda.');
      }
    } else {
      try {
        const response = await axios.post('https://localhost:7089/api/sale', sale, {
          headers: {
            Authorization: `Bearer ${token}`,
          },
        });

        const newSaleData = response.data;
        setSales([...sales, newSaleData]);
        setSale({
          cut: '',
          quantity: '',
          description: '',
          discount: '',
          price: '',
        });
        toast.success('Venda cadastrada com sucesso.');
      } catch (error) {
        console.error('Erro ao cadastrar venda:', error);
        toast.error('Erro ao cadastrar venda.');
      }
    }
  };

  const handleEdit = (saleId) => {
    const saleToEdit = sales.find((sale) => sale.id === saleId);

    if (saleToEdit) {
      setSale({
        id: saleToEdit.id,
        cut: saleToEdit.cut,
        quantity: saleToEdit.quantity,
        description: saleToEdit.description,
        discount: saleToEdit.discount,
        price: saleToEdit.price,
      });

      setIsEditing(true);
    } else {
      toast.error('Venda não encontrada para edição.');
    }
  };

  const handleDelete = async (saleId) => {
    try {
      const token = localStorage.getItem('token');
      await axios.delete(`https://localhost:7089/api/sale/${saleId}`, {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      });
      setSales(sales.filter((sale) => sale.id !== saleId));
      toast.success('Venda excluída com sucesso.');
    } catch (error) {
      console.error('Erro ao excluir venda:', error);
      toast.error('Erro ao excluir venda.');
    }
  };

  return (
    <>
      <div className='all'>
        <div className="sale-container">
          <h1>{isEditing ? "Editar Sale" : "Cadastro de Sale"}</h1>
          <form onSubmit={handleSubmit}>
            <div className="form-group">
              <label htmlFor="cut">Cortes:</label>
              <input type="text" id="cut" name="cut" value={sale.cut} onChange={handleInputChange} className='vInput' />
              <label htmlFor="quantity">Quantity:</label>
              <input type="number" id="quantity" name="quantity" value={sale.quantity} onChange={handleInputChange} className='vInput' />
              <label htmlFor="description">Description:</label>
              <input type="text" id="description" name="description" value={sale.description} onChange={handleInputChange} className='vInput' />
              <label htmlFor="discount">Discount:</label>
              <input type="number" id="discount" name="discount" value={sale.discount} onChange={handleInputChange} className='vInput' />
              <label htmlFor="price">Price:</label>
              <input type="number" id="price" name="price" value={sale.price} onChange={handleInputChange} className='vInput' />
            </div>
            <div className="ctas">
              <button type="submit" className="cta-button">
                {isEditing ? "Salvar Edição" : "Cadastrar Sale"}
              </button>
              <LogoutButton />
            </div>
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
      </div>
    </>
  );
}

export default SaleForm;
