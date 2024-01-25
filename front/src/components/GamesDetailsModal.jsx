import React, { useState, useEffect } from 'react';
import '../css/GameDetailsModal.css';

const GameDetailsModal = ({ game, onClose }) => {
  const [reviews, setReviews] = useState([]);
  const [newReview, setNewReview] = useState({ rating: 0, comment: '' });
  const [collections, setCollections] = useState([]);
  const [selectedCollection, setSelectedCollection] = useState('');

  useEffect(() => {
    const fetchReviews = async () => {
      try {
        const response = await fetch(`http://localhost:5163/Review/GetReviewsOfGame/${game.title}`, {
          method: 'GET',
          headers: {
            'Content-Type': 'application/json',
            Authorization: `Bearer ${localStorage.getItem('token')}`
          },
        });

        if (response.ok) {
          const reviewsData = await response.json();
          setReviews(reviewsData);
        } else {
          console.error('Error fetching reviews:', response.statusText);
          const responseText = await response.text();
          console.log('Response text:', responseText);
        }
      } catch (error) {
        console.error('Error fetching reviews:', error);
      }
    };

    fetchReviews();
  }, [game.title]);

  useEffect(() => {
    const fetchReviews = async () => {
      // ... (unchanged)
    };

  
    const fetchCollections = async () => {
      try {
        const response = await fetch('http://localhost:5163/GamingList/GetGamingListsWithGames', {
          method: 'GET',
          headers: {
            'Content-Type': 'application/json',
            Authorization: `Bearer ${localStorage.getItem('token')}`
          },
        });

        if (response.ok) {
          const collectionsData = await response.json();
          setCollections(collectionsData);
          console.log(collectionsData)
        } else {
          console.error('Error fetching collections:', response.statusText);
        }
      } catch (error) {
        console.error('Error fetching collections:', error);
      }
    };

    fetchReviews();
    fetchCollections();
  }, [game.title]);

  const handleRatingChange = (event) => {
    setNewReview({ ...newReview, rating: parseFloat(event.target.value) });
  };

  const handleCommentChange = (event) => {
    setNewReview({ ...newReview, comment: event.target.value });
  };

  const handleSubmitReview = async () => {
    if (newReview.rating >= 1 && newReview.rating <= 5 && newReview.comment.trim() !== '') {
      try {
        const response = await fetch(`http://localhost:5163/Review/PostReview/${encodeURIComponent(newReview.comment)}/${newReview.rating}/${encodeURIComponent(game.title)}`, {
          method: 'POST',
          headers: {
            'Content-Type': 'application/json',
            Authorization: `Bearer ${localStorage.getItem('token')}`
          },
        });

        if (response.ok) {
          // Fetch the updated reviews after adding the new review
          const reviewsResponse = await fetch(`http://localhost:5163/Review/GetReviewsOfGame/${game.title}`, {
            method: 'GET',
            headers: {
              'Content-Type': 'application/json',
              Authorization: `Bearer ${localStorage.getItem('token')}`
            },
          });

          if (reviewsResponse.ok) {
            const reviewsData = await reviewsResponse.json();
            setReviews(reviewsData);
            window.location.reload()
          } else {
            console.error('Error fetching reviews:', reviewsResponse.statusText);
          }
          
          // Clear the form
          setNewReview({ rating: 0, comment: '' });
        } else {
          console.error('Error submitting review:', response.statusText);
        }
      } catch (error) {
        console.error('Error submitting review:', error);
      }
    } else {
      alert('Please enter a valid rating (1-5) and a non-empty comment.');
    }
  };
  const handleCollectionChange = (event) => {
    setSelectedCollection(event.target.value);
  };

  const handleAddToCollection = async () => {
    if (selectedCollection !== '') {
      try {
        // Make a POST request to an endpoint that handles adding the game to the selected collection
        const response = await fetch(`http://localhost:5163/GamingList/AddGameToGamingList/${game.title}/${encodeURIComponent(selectedCollection)}`, {
          method: 'PUT',
          headers: {
            'Content-Type': 'application/json',
            Authorization: `Bearer ${localStorage.getItem('token')}`
          },
        });

        if (response.ok) {
          // Notify user about successful addition to the collection
          alert(`Game added to collection: ${selectedCollection}`);
        } else {
          console.error('Error adding to collection:', response.statusText);
        }
      } catch (error) {
        console.error('Error adding to collection:', error);
      }
    } else {
      // Handle validation error
      alert('Please select a collection.');
    }
  };
  
  return (
    <div>
      {/* Backdrop */}
      <div className="backdrop"></div>

      {/* Modal */}
      <div className='game-details-modal'>
        {/* Left Section */}
        <div className="left-section">
        <div className='add-to-collection-form'>
            <label>
              Select Collection:
              <select value={selectedCollection} name="select-collection" onChange={handleCollectionChange}>
                <option value='' disabled>Select a collection</option>
                {collections.map((collection) => (
                  <option key={collection.id} value={collection.name}>
                    {collection.name}
                  </option>
                ))}
              </select>
            </label>
            <button name='buttonAddGameToCollection'type="button" onClick={handleAddToCollection}>
              Add to Collection
            </button>
          </div>
          <h2>{game.title}</h2>
          <img src={require("./" + game.image)} style={{ maxHeight: '250px' }} alt={game.title} />
          <p>Description: {game.description}</p>
          <p>Genre: {game.genre}</p>
          <p>Rating: {game.rating}</p>
        </div>

        {/* Right Section */}
        <div className="right-section">
          <h3>Reviews</h3>
          <ul className="comments-list">
            {reviews
              .sort((a, b) => b.rating - a.rating)
              .map((review) => (
                <li key={review.id} className='Review'>
                  <p>Rating: {review.rating}</p>
                  <p>Comment: {review.comment}</p>
                </li>
              ))}
          </ul>

          {/* Form for Adding a New Review */}
          <form>
            <label>
              Your Rating:
              <input
                type="number"
                name='my-rating'
                min="1"
                max="5"
                step="1"
                value={newReview.rating}
                onChange={handleRatingChange}
              />
            </label>
            <label>
              Your Comment:
              <textarea
                name='my-comment'
                className='my-comment'
                value={newReview.comment}
                onChange={handleCommentChange}
              />
            </label>
            <button name='submit-review' type="button" onClick={handleSubmitReview}>
              Submit Review
            </button>
          </form>
        </div>

        {/* Close Button */}
        <button onClick={onClose}>X</button>
      </div>
    </div>
  );
};

export default GameDetailsModal;
