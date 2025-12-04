import React, { useState, useEffect, useCallback } from 'react';
import { valeraAPI } from '../services/api';

const ValeraStats = ({ valeraId, onBack }) => {
  const [valera, setValera] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');

  // useCallback чтобы функция не пересоздавалась при каждом рендере
  const loadValera = useCallback(async () => {
    try {
      setError('');
      const response = await valeraAPI.getById(valeraId);
      setValera(response.data);
    } catch (error) {
      console.error('Error loading valera:', error);
      setError('Failed to load valera data');
    } finally {
      setLoading(false);
    }
  }, [valeraId]);

  useEffect(() => {
    loadValera();
  }, [loadValera]);

  const handleAction = async (action) => {
    try {
      await valeraAPI[action](valeraId);
      await loadValera();
    } catch (error) {
      console.error(`Error performing ${action}:`, error);
      alert('Action failed! Check conditions.');
    }
  };

  const ProgressBar = ({ value, max, color, label }) => (
    <div style={styles.statItem}>
      <div style={styles.statHeader}>
        <span style={styles.statLabel}>{label}</span>
        <span style={styles.statValue}>{value}/{max}</span>
      </div>
      <div style={styles.progressBar}>
        <div 
          style={{
            ...styles.progress,
            width: `${(value / max) * 100}%`,
            background: color
          }}
        ></div>
      </div>
    </div>
  );

  if (loading) {
    return (
      <div style={styles.loading}>
        Loading Valera #{valeraId}...
      </div>
    );
  }

  if (error || !valera) {
    return (
      <div style={styles.container}>
        <button style={styles.backButton} onClick={onBack}>
          ← Back to List
        </button>
        <div style={styles.error}>
          {error || 'Valera not found'}
          <button onClick={loadValera} style={styles.retryButton}>
            Retry
          </button>
        </div>
      </div>
    );
  }

  const isAlive = valera.health > 0;
  const canWork = valera.mana < 50 && valera.fatigue < 10;
  const canDrinkWine = valera.money >= 20;
  const canGoToBar = valera.money >= 100;
  const canDrinkWithMarginals = valera.money >= 150;

  const actions = [
    { name: 'work', label: '💼 Go to zavod', disabled: !canWork || !isAlive },
    { name: 'nature', label: '🌳 Nature', disabled: !isAlive },
    { name: 'wine', label: '🍷 Wine & TV', disabled: !canDrinkWine || !isAlive },
    { name: 'bar', label: '🍺 Bar', disabled: !canGoToBar || !isAlive },
    { name: 'marginals', label: '🥃 Marginals', disabled: !canDrinkWithMarginals || !isAlive },
    { name: 'sing', label: '🎤 Sing', disabled: !isAlive },
    { name: 'sleep', label: '🛌 Sleep', disabled: !isAlive },
  ];

  return (
    <div style={styles.container}>
      <button style={styles.backButton} onClick={onBack}>
        ← Back to List
      </button>

      <h1 style={styles.title}>📊 Valera #{valera.id} Statistics</h1>

      <div style={styles.statsContainer}>
        <div style={styles.statsGrid}>
          <div style={styles.column}>
            <ProgressBar value={valera.health} max={100} color="#dc3545" label="❤️ Health" />
            <ProgressBar value={valera.mana} max={100} color="#ffc107" label="🍺 Alcohol" />
            <ProgressBar value={valera.cheerfulness + 10} max={20} color="#28a745" label="😊 Cheerfulness" />
          </div>
          
          <div style={styles.column}>
            <ProgressBar value={valera.fatigue} max={100} color="#17a2b8" label="😪 Fatigue" />
            
            <div style={styles.moneySection}>
              <h3>💰 Money</h3>
              <div style={styles.moneyAmount}>${valera.money}</div>
            </div>
            
            <div style={styles.statusSection}>
              <h3>Status</h3>
              <div style={{
                ...styles.status,
                ...(isAlive ? styles.statusAlive : styles.statusDead)
              }}>
                {isAlive ? '✅ ALIVE AND KICKING!' : '💀 REST IN PEACE'}
              </div>
            </div>
          </div>
        </div>
      </div>

      <div style={styles.actionsSection}>
        <h2 style={styles.actionsTitle}>🎮 Actions</h2>
        <div style={styles.actionsGrid}>
          {actions.map(action => (
            <button
              key={action.name}
              style={{
                ...styles.actionButton,
                ...(action.disabled && styles.actionButtonDisabled)
              }}
              onClick={() => handleAction(action.name)}
              disabled={action.disabled}
            >
              {action.label}
            </button>
          ))}
        </div>
      </div>
    </div>
  );
};

const styles = {
  container: {
    maxWidth: '1000px',
    margin: '0 auto',
    padding: '20px'
  },
  backButton: {
    padding: '10px 20px',
    background: '#6c757d',
    color: 'white',
    border: 'none',
    borderRadius: '8px',
    cursor: 'pointer',
    marginBottom: '20px'
  },
  title: {
    textAlign: 'center',
    marginBottom: '30px',
    color: 'white',
    fontSize: '2.2em',
    textShadow: '2px 2px 4px rgba(0,0,0,0.3)'
  },
  statsContainer: {
    background: 'white',
    borderRadius: '12px',
    padding: '30px',
    boxShadow: '0 4px 6px rgba(0,0,0,0.1)',
    marginBottom: '30px'
  },
  statsGrid: {
    display: 'grid',
    gridTemplateColumns: '1fr 1fr',
    gap: '40px'
  },
  column: {
    display: 'flex',
    flexDirection: 'column',
    gap: '25px'
  },
  statItem: {
    marginBottom: '15px'
  },
  statHeader: {
    display: 'flex',
    justifyContent: 'space-between',
    alignItems: 'center',
    marginBottom: '8px'
  },
  statLabel: {
    fontWeight: '600',
    color: '#333',
    fontSize: '16px'
  },
  statValue: {
    color: '#666',
    fontWeight: '500'
  },
  progressBar: {
    background: '#e9ecef',
    borderRadius: '10px',
    height: '20px',
    overflow: 'hidden'
  },
  progress: {
    height: '100%',
    borderRadius: '10px',
    transition: 'width 0.3s ease'
  },
  moneySection: {
    textAlign: 'center',
    padding: '20px',
    background: '#f8f9fa',
    borderRadius: '8px'
  },
  moneyAmount: {
    fontSize: '2.5em',
    fontWeight: 'bold',
    color: '#28a745'
  },
  statusSection: {
    textAlign: 'center',
    padding: '20px',
    background: '#f8f9fa',
    borderRadius: '8px'
  },
  status: {
    padding: '10px 15px',
    borderRadius: '6px',
    fontWeight: 'bold',
    fontSize: '1.1em'
  },
  statusAlive: {
    background: '#d4edda',
    color: '#155724'
  },
  statusDead: {
    background: '#f8d7da',
    color: '#721c24'
  },
  actionsSection: {
    background: 'white',
    borderRadius: '12px',
    padding: '30px',
    boxShadow: '0 4px 6px rgba(0,0,0,0.1)'
  },
  actionsTitle: {
    textAlign: 'center',
    marginBottom: '20px',
    color: '#333'
  },
  actionsGrid: {
    display: 'grid',
    gridTemplateColumns: 'repeat(auto-fit, minmax(180px, 1fr))',
    gap: '12px'
  },
  actionButton: {
    padding: '15px 10px',
    fontSize: '14px',
    background: '#007bff',
    color: 'white',
    border: 'none',
    borderRadius: '8px',
    cursor: 'pointer',
    fontWeight: '600'
  },
  actionButtonDisabled: {
    background: '#6c757d',
    cursor: 'not-allowed',
    opacity: 0.6
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
  }
};

export default ValeraStats;