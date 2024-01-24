import React, { useState, useEffect } from 'react';
import axios from 'axios';
import GameCard from './GameCard';
import '../css/MyGames.css';
import GameDetailsModal from './GamesDetailsModal';
import CreateCollectionModal from './CreateCollectionModal';

const MyGames = () => {
  const [collectionsData, setCollectionsData] = useState([]);
  const [selectedGame, setSelectedGame] = useState(null);
  const [isCreateModalOpen, setIsCreateModalOpen] = useState(false);

  useEffect(() => {
    const fetchData = async () => {
      try {
        // Fetch collections data from the server endpoint
        const response = await axios.get('http://localhost:5163/GamingList/GetGamingListsWithGames', {
          headers: {
            'Content-Type': 'application/json',
            Authorization: `Bearer ${localStorage.getItem('token')}`
          },
        });
        
        // Iterate over collections and games to fetch reviews and calculate ratings
        const collectionsWithReviews = await Promise.all(
          response.data.map(async (collection) => {
            const gamesWithReviews = await Promise.all(
              collection.gamesInGamingList.map(async (game) => {
                const reviewsResponse = await axios.get(`http://localhost:5163/Review/GetReviewsOfGame/${game.title}`, {
                  headers: {
                    'Content-Type': 'application/json',
                    Authorization: `Bearer ${localStorage.getItem('token')}`
                  },
                });

                if (reviewsResponse.data.length > 0) {
                  const averageRating = reviewsResponse.data.reduce((sum, review) => sum + review.rating, 0) / reviewsResponse.data.length;
                  return { ...game, rating: averageRating };
                } else {
                  return { ...game, rating: 0 };
                }
              })
            );

            return { ...collection, gamesInGamingList: gamesWithReviews };
          })
        );

        setCollectionsData(collectionsWithReviews);
      } catch (error) {
        console.error('Error fetching data:', error);
      }
    };

    fetchData();
  }, []);

  useEffect(() => {
    console.log('collectionsData changed:', collectionsData);
  }, [collectionsData]);

  const handleCardClick = (game) => {
    setSelectedGame(game);
  };

  const handleCloseModal = () => {
    setSelectedGame(null);
  };

  const handleOpenCreateModal = () => {
    setIsCreateModalOpen(true);
  };

  const handleCloseCreateModal = () => {
    setIsCreateModalOpen(false);
  };

  const handleAddCollection = (newCollectionName) => {
    const newCollection = {
      name: newCollectionName,
      gamesInGamingList: [], // Ensure the correct property name
    };

    setCollectionsData([...collectionsData, newCollection]);
  };
  useEffect(()=>{
    console.log(collectionsData)
  },[collectionsData])
  const existingToken = localStorage.getItem('token');
  if (!existingToken) {
    // If a token is present, redirect the user (adjust the redirect path as needed)
    window.location.href = '/login-register';
    return null; // Optionally, you can return null to prevent rendering the login/register form
  }

  const handleDeleteCollection = async (collectionName) => {
    try {
      // Make an HTTP request to delete the collection
      await axios.delete(`http://localhost:5163/GamingList/DeleteGamingList/${encodeURIComponent(collectionName)}`, {
        headers: {
          'Content-Type': 'application/json',
          Authorization: `Bearer ${localStorage.getItem('token')}`
        },
      });
  
      // Update state to remove the deleted collection
      setCollectionsData((prevCollections) =>
        prevCollections.filter((collection) => collection.name !== collectionName)
      );
    } catch (error) {
      console.error('Error deleting collection:', error);
    }
  };
  
  return (
    <div className="games-container">
      <button name='AddCollection' onClick={handleOpenCreateModal}>Add Collection</button>

      {collectionsData.sort((a, b) => b.gamesInGamingList.length - a.gamesInGamingList.length).map((collection) => (
        <div key={collection.name}>
          <h1 name={collection.name}>
            {collection.name}
            <button className='delete-button' onClick={() => handleDeleteCollection(collection.name)}>
              Delete
            </button>
          </h1>
          <div className="cards-container">
            {collection.gamesInGamingList && (
              <div className="cards">
                {collection.gamesInGamingList.map((game) => (
                  <GameCard key={game.gameId} {...game} onClick={() => handleCardClick(game)} />
                ))}
              </div>
            )}
            {!collection && <div>Loading data...</div>}
          </div>
        </div>
      ))}

      {selectedGame && <GameDetailsModal game={selectedGame} onClose={handleCloseModal} />}
      {isCreateModalOpen && (
        <CreateCollectionModal onClose={handleCloseCreateModal} onAddCollection={handleAddCollection} />
      )}
    </div>
  );
};

export default MyGames;
