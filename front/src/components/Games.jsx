import React, { useState, useEffect } from 'react';
import GameCard from './GameCard';
import '../css/Games.css';
import GameDetailsModal from './GamesDetailsModal';
import Filter from './Filter';

const Games = () => {
  const [gamesData, setGamesData] = useState([]);
  const [filteredGames, setFilteredGames] = useState([]);
  const [selectedGame, setSelectedGame] = useState(null);

  useEffect(() => {
    const fetchData = async () => {
      try {
        // Fetch games from the API endpoint
        const response = await fetch(`http://localhost:5163/Game/GetGames`, {
          method: 'GET',
          headers: {
            'Content-Type': 'application/json',
            Authorization: `Bearer ${localStorage.getItem('token')}`
          },
        });

        if (response.ok) {
          const games = await response.json();
          const gamesWithRating = await Promise.all(
            games.map(async (game) => {
              const reviewsResponse = await fetch(`http://localhost:5163/Review/GetReviewsOfGame/${game.title}`, {
                method: 'GET',
                headers: {
                  'Content-Type': 'application/json',
                  Authorization: `Bearer ${localStorage.getItem('token')}`
                },
              });
              if (reviewsResponse.ok) {
                const reviews = await reviewsResponse.json();
                const averageRating = reviews.length > 0
                  ? reviews.reduce((sum, review) => sum + review.rating, 0) / reviews.length
                  : 0;
                return { ...game, rating: averageRating };
              } else {
                console.error(`Error fetching reviews for ${game.title}:`, reviewsResponse.statusText);
                return { ...game, rating: 0 };
              }
            })
          );

          setGamesData(gamesWithRating);
          setFilteredGames(gamesWithRating);
        } else {
          console.error('Error fetching data:', response.statusText);
        }
      } catch (error) {
        console.error('An error occurred during data fetching:', error);
      }
    };

    fetchData();
  }, []);
  
  
  

  const handleCardClick = (game) => {
    setSelectedGame(game);
  };

  const handleCloseModal = () => {
    setSelectedGame(null);
  };

  const handleFilterChange = (filterCriteria) => {
    const filtered = gamesData.filter((game) => {
      const title = game.title ? game.title.toLowerCase() : '';
      const platform = game.platform ? game.platform.toLowerCase() : '';
      const genre = game.genre ? game.genre.toLowerCase() : '';

      return (
        title.includes(filterCriteria.title.toLowerCase()) &&
        game.rating >= filterCriteria.minRating &&
        platform.includes(filterCriteria.platform.toLowerCase()) &&
        genre.includes(filterCriteria.genre.toLowerCase())
      );
    });

    setFilteredGames(filtered);
  };

  useEffect(() => {
    console.log('filteredGames changed:', filteredGames);
  }, [filteredGames]);

  const existingToken = localStorage.getItem('token');
  if (!existingToken) {
    window.location.href = '/login-register';
    return null; 
  }

  return (
    <div>
      <div>
        <Filter onFilterChange={handleFilterChange} />
      </div>
      {filteredGames.length > 0 ? (
        <div className='cards'>
          {filteredGames.map((game) => (
            <GameCard key={game.id} {...game} onClick={() => handleCardClick(game)} />
          ))}
        </div>
      ) : (
        <div>No matching games found.</div>
      )}
      {!gamesData && <div>Loading data...</div>}
      {selectedGame && <GameDetailsModal game={selectedGame} onClose={handleCloseModal} />}
    </div>
  );
};

export default Games;