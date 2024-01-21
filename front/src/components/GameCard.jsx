import React from 'react';
import '../css/GameCard.css';

const GameCard = ({ title, genre, image, onClick }) => { 
  const cardStyle = {
  backgroundImage: `url(${require("./"+image)})`,
  backgroundPosition: 'center',
};
  return (
    <div style={cardStyle} className='game-card' onClick={onClick}>
      <p className='game-name'>{title}</p>
      <div className='details'>
        <p>Genre: {genre}</p>
      </div>
    </div>
  );
};

export default GameCard;