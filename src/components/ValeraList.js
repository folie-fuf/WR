import React, { useState, useEffect } from 'react';
import { valeraAPI } from '../services/api';

const ValeraList = ({ onValeraSelect }) => {
  const [valeras, setValeras] = useState([]);
  const [searchTerm, setSearchTerm] = useState('');
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');

  useEffect(() => {
    loadValeras();
  }, []);

  const loadValeras = async () => {
    try {
      setError('');
      const response = await valeraAPI.getAll();
      setValeras(response.data);
    } catch (error) {
      console.error('Error loading valeras:', error);
      setError('Failed to load valeras. Make sure the backend is running on http://localhost:5291');
    } finally {
      setLoading(false);
    }
  };

  const handleCreateValera = async () => {
    const newValera = {
      health: 100,
      mana: 0,
      cheerfulness: 0,
      fatigue: 0,
      money: 100
    };
    
    try {
      await valeraAPI.create(newValera);
      loadValeras(); // Reload the list
    } catch (error) {
      console.error('Error creating valera:', error);
      setError('Failed to create valera');
    }
  };

  const filteredValeras = valeras.filter(valera =>
    `Valera ${valera.id}`.toLowerCase().includes(searchTerm.toLowerCase())
  );

  if (loading) {
    return (
      <div style={styles.loading}>
        Loading Valeras...
      </div>
    );
  }

  return (
    <div style={styles.container}>
      <h1 style={styles.title}>🎮 Virtual Valera Manager</h1>
      
      {error && (
        <div style={styles.error}>
          {error}
          <button onClick={loadValeras} style={styles.retryButton}>
            Retry
          </button>
        </div>
      )}
      
      <div style={styles.controls}>
        <input
          type="text"
          placeholder="🔍 Search Valeras..."
          style={styles.searchInput}
          value={searchTerm}
          onChange={(e) => setSearchTerm(e.target.value)}
        />
        <button 
          style={styles.createButton}
          onClick={handleCreateValera}
        >
          ➕ Create Valera
        </button>
      </div>

      <div style={styles.grid}>
        {filteredValeras.map(valera => {
          // Вычисляем IsAlive на основе здоровья
          const isAlive = valera.health > 0;
          return (
            <div
              key={valera.id}
              style={styles.card}
              onClick={() => onValeraSelect(valera.id)}
            >
              <h3 style={styles.cardTitle}>Valera #{valera.id}</h3>
              <div style={styles.stats}>
                <p>❤️ Health: {valera.health}%</p>
                <p>🍺 Alcohol: {valera.mana}%</p>
                <p>😊 Mood: {valera.cheerfulness}</p>
                <p>😪 Fatigue: {valera.fatigue}%</p>
                <p>💰 Money: ${valera.money}</p>
              </div>
              <div style={{
                ...styles.status,
                ...(isAlive ? styles.statusAlive : styles.statusDead)
              }}>
                {isAlive ? '✅ ALIVE' : '💀 DEAD'}
              </div>
            </div>
          );
        })}
      </div>

      {filteredValeras.length === 0 && !error && (
        <div style={styles.empty}>
          No Valeras found. Create your first Valera!
        </div>
      )}
    </div>
  );
};

const styles = {
  container: {
    maxWidth: '1200px',
    margin: '0 auto',
    padding: '20px'
  },
  title: {
    textAlign: 'center',
    marginBottom: '30px',
    color: 'white',
    fontSize: '2.5em',
    textShadow: '2px 2px 4px rgba(0,0,0,0.3)'
  },
  controls: {
    display: 'flex',
    gap: '15px',
    marginBottom: '30px',
    alignItems: 'center'
  },
  searchInput: {
    flex: 1,
    padding: '12px 16px',
    border: '2px solid #ddd',
    borderRadius: '8px',
    fontSize: '16px'
  },
  createButton: {
    padding: '12px 24px',
    background: '#007bff',
    color: 'white',
    border: 'none',
    borderRadius: '8px',
    cursor: 'pointer',
    fontSize: '16px',
    fontWeight: '600'
  },
  grid: {
    display: 'grid',
    gridTemplateColumns: 'repeat(auto-fill, minmax(300px, 1fr))',
    gap: '20px'
  },
  card: {
    background: 'white',
    border: '1px solid #e0e0e0',
    borderRadius: '12px',
    padding: '20px',
    cursor: 'pointer',
    transition: 'all 0.3s'
  },
  cardTitle: {
    margin: '0 0 15px 0',
    color: '#333',
    fontSize: '1.4em',
    borderBottom: '2px solid #f0f0f0',
    paddingBottom: '10px'
  },
  stats: {
    color: '#555'
  },
  status: {
    marginTop: '15px',
    padding: '8px 12px',
    borderRadius: '6px',
    fontWeight: 'bold',
    textAlign: 'center'
  },
  statusAlive: {
    background: '#d4edda',
    color: '#155724'
  },
  statusDead: {
    background: '#f8d7da',
    color: '#721c24'
  },
  loading: {
    textAlign: 'center',
    padding: '60px 20px',
    fontSize: '18px',
    color: 'white'
  },
  error: {
    background: '#f8d7da',
    color: '#721c24',
    padding: '15px',
    borderRadius: '8px',
    marginBottom: '20px',
    display: 'flex',
    justifyContent: 'space-between',
    alignItems: 'center'
  },
  retryButton: {
    background: '#dc3545',
    color: 'white',
    border: 'none',
    padding: '8px 16px',
    borderRadius: '4px',
    cursor: 'pointer'
  },
  empty: {
    textAlign: 'center',
    padding: '60px 20px',
    color: 'white',
    fontSize: '18px',
    background: 'rgba(255,255,255,0.1)',
    borderRadius: '8px',
    marginTop: '20px'
  }
};

export default ValeraList;