import React, { useState } from 'react';
import ValeraList from './components/ValeraList';
import ValeraStats from './components/ValeraStats';
import './App.css';

function App() {
  const [currentView, setCurrentView] = useState('list');
  const [selectedValeraId, setSelectedValeraId] = useState(null);

  const handleValeraSelect = (valeraId) => {
    setSelectedValeraId(valeraId);
    setCurrentView('stats');
  };

  const handleBackToList = () => {
    setCurrentView('list');
    setSelectedValeraId(null);
  };

  return (
    <div className="App">
      {currentView === 'list' && (
        <ValeraList onValeraSelect={handleValeraSelect} />
      )}
      
      {currentView === 'stats' && selectedValeraId && (
        <ValeraStats 
          valeraId={selectedValeraId} 
          onBack={handleBackToList}
        />
      )}
    </div>
  );
}

export default App;