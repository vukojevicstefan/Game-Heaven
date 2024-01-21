import React, { useState } from 'react';
import '../css/CreateCollectionModal.css';
import axios from 'axios';

const CreateCollectionModal = ({ onClose, onAddCollection }) => {
  const [newCollectionName, setNewCollectionName] = useState('');

  const handleCreateCollection = async () => {
    if (newCollectionName.trim() === '') {
      alert('Please enter a collection name.');
      return;
    }

    try {
      // Make a POST request to create a new collection
      const response = await axios.post(
        `http://localhost:5163/GamingList/CreateNewGamingList/${encodeURIComponent(newCollectionName)}`,
        null,
        {
          headers: {
            'Content-Type': 'application/json',
            Authorization: `Bearer ${localStorage.getItem('token')}`
          },
        }
      );

      if (response.statusText=='OK') {
        onAddCollection(newCollectionName);
      } else {
        console.error('Error creating collection:', response.statusText);
      }
    } catch (error) {
      console.error('Error creating collection, caught error:', error);
    }
    onClose();
  };

  return (
    <div className="modal-overlay">
      <div className="modal-content">
        <h2>Create a New Collection</h2>
        <input
          type="text"
          placeholder="Enter Collection Name"
          value={newCollectionName}
          onChange={(e) => setNewCollectionName(e.target.value)}
        />
        <div className='buttons'>
          <button onClick={handleCreateCollection}>Create</button>
          <button onClick={onClose}>Cancel</button>
        </div>
      </div>
    </div>
  );
};

export default CreateCollectionModal;
